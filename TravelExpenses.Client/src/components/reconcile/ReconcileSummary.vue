<template>
  <v-container>
    <v-card>
      <v-card-title class="pb-0">
        <v-layout row align-center>
          <v-avatar class="mr-2" size="55" color="primary">
            <v-icon dark large>description</v-icon>
          </v-avatar>
          <div class="ml-1">
            <h3>Reconcile summary for:</h3>
            <h4>{{ getLocationString() }}</h4>
            <h4>
              {{
                `${currency ? currency.isoCode : ''} - ${
                  currency ? currency.currencyName : ''
                }`
              }}
            </h4>
          </div>
        </v-layout>
      </v-card-title>
      <v-card-text>
        <v-divider class="mb-3"></v-divider>
        <v-layout row>
          <v-flex grow>
            <h3>{{ `${currency ? currency.isoCode : ''} withdrawn:` }}</h3>
          </v-flex>
          <v-flex shrink>
            <h3 class="text-xs-right">
              {{
                reconcileSummary.totalWithdrawn
                  ? formatNumber(reconcileSummary.totalWithdrawn)
                  : ''
              }}
            </h3>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex grow>
            <h3>{{ `${currency ? currency.isoCode : ''} spent:` }}</h3>
          </v-flex>
          <v-flex shrink>
            <h3>
              {{
                reconcileSummary.totalSpent
                  ? formatNumber(reconcileSummary.totalSpent)
                  : '0'
              }}
            </h3>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex grow>
            <h3>Cash expected:</h3>
          </v-flex>
          <v-flex shrink>
            <h3>{{ formatNumber(cashShouldBe) }}</h3>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex grow>
            <h3>Cash actual:</h3>
          </v-flex>
          <v-flex shrink>
            <h3>{{ formatNumber(cashOnHand) }}</h3>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex grow>
            <h3>Difference:</h3>
          </v-flex>
          <v-flex shrink>
            <h3>{{ formatNumber(difference) }}</h3>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex grow>
            <h3>Previous loss/gain:</h3>
          </v-flex>
          <v-flex shrink>
            <h3>{{ formatNumber(reconcileSummary.totalLossGain * -1) }}</h3>
          </v-flex>
        </v-layout>
        <v-divider class="my-3"></v-divider>
        <div v-if="numbersMatch">
          <h3>You cash on-hand is correct.</h3>
          <v-layout row>
            <v-flex grow>
              <h2 style="display: inline" class="green--text">
                Reconciliation complete
              </h2>
            </v-flex>
            <v-flex shrink>
              <v-icon color="green">done_outline</v-icon>
            </v-flex>
          </v-layout>
        </div>
        <div v-else>
          <v-layout row align-center>
            <v-flex grow>
              <h3 class="red--text mr-2" style="display: inline">
                {{ resultString }}
              </h3>
            </v-flex>
            <v-flex shrink>
              <v-icon large color="red">{{
                haveNetGain ? 'trending_up' : 'trending_down'
              }}</v-icon>
            </v-flex>
          </v-layout>
        </div>
      </v-card-text>
    </v-card>
    <v-layout v-if="!numbersMatch" row justify-center>
      <v-btn dark color="primary" class="mt-3" @click="navToInvestigation"
        >Investigate</v-btn
      >
    </v-layout>
  </v-container>
</template>

<script>
import Windows from '@/common/enums/ReconcileWindows.js'
import { mapState, mapGetters } from 'vuex'
import { toLocaleStringWithEndingZero } from '@/common/StringUtilities.js'

export default {
  methods: {
    navToInvestigation() {
      this.$store.dispatch(
        'Reconcile/setReconcileWindowId',
        Windows.investigation
      )
    },
    formatNumber(numValue) {
      return toLocaleStringWithEndingZero(numValue)
    },
    getLocationString() {
      if (!this.location) {
        return ''
      }

      const country = this.findCountry(this.location.countryId)
      let countryName = ''
      if (country) {
        countryName = country.countryName
      }

      return `${this.location.locationName}, ${countryName}`
    }
  },
  computed: {
    ...mapState('Reconcile', [
      'location',
      'currency',
      'cashOnHand',
      'reconcileSummary'
    ]),
    ...mapGetters('Reconcile', [
      'cashShouldBe',
      'difference',
      'haveNetGain',
      'numbersMatch',
      'resultString'
    ]),
    ...mapGetters('Country', ['findCountry'])
  }
}
</script>

<style scoped>
h3 {
  display: inline;
}
</style>
