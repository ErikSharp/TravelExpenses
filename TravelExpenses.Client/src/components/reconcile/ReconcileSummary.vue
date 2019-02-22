<template>
  <div>
    <v-card>
      <v-card-title class="pb-0">
        <v-avatar class="mr-2" size="55" color="primary">
          <v-icon dark large>description</v-icon>
        </v-avatar>
        <h3>{{ `Summary for ${locationName} (${currencyObj.isoCode})` }}</h3>
      </v-card-title>
      <v-card-text>
        <v-divider class="mb-3"></v-divider>
        <v-layout row>
          <v-flex grow>
            <h3>{{ `Total ${currencyObj.currencyName} withdrawn:` }}</h3>
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
            <h3>{{ `Total ${currencyObj.currencyName} spent:` }}</h3>
          </v-flex>
          <v-flex shrink>
            <h3>
              {{
                reconcileSummary.totalSpent
                  ? formatNumber(reconcileSummary.totalSpent)
                  : ''
              }}
            </h3>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex grow>
            <h3>Cash on-hand should be:</h3>
          </v-flex>
          <v-flex shrink>
            <h3>{{ formatNumber(shouldBe) }}</h3>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex grow>
            <h3>Cash on-hand actual:</h3>
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
            <h3 :class="{ 'red--text': difference !== 0 }">
              {{ formatNumber(difference) }}
            </h3>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex grow>
            <h3>Accounted for loss / gain:</h3>
          </v-flex>
          <v-flex shrink>
            <h3>todo</h3>
          </v-flex>
        </v-layout>
        <v-divider class="my-3"></v-divider>
        <div v-if="difference === 0">
          <h3>You cash on-hand is correct.</h3>
          <h2 class="green--text">Reconciliation complete</h2>
        </div>
        <div v-else>
          <v-layout row align-center>
            <h3 class="red--text mr-2" style="display: inline">
              {{
                `You ${difference > 0 ? 'have' : 'are'} ${formatNumber(
                  Math.abs(difference)
                )} ${currencyObj.isoCode} ${
                  difference > 0 ? 'too much' : 'short'
                }`
              }}
            </h3>
            <v-icon large color="red">{{
              difference > 0 ? 'trending_up' : 'trending_down'
            }}</v-icon>
          </v-layout>
        </div>
      </v-card-text>
    </v-card>
    <v-layout v-if="difference !== 0" row justify-center>
      <v-btn dark color="primary" class="mt-3" @click="navToInvestigation"
        >Investigate</v-btn
      >
    </v-layout>
  </div>
</template>

<script>
import Windows from '@/common/enums/ReconcileWindows.js'
import { mapState } from 'vuex'
import round from 'lodash/round'
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
    }
  },
  computed: {
    ...mapState('Reconcile', [
      'location',
      'currency',
      'cashOnHand',
      'reconcileSummary'
    ]),
    shouldBe() {
      if (!this.reconcileSummary) {
        return 0
      }

      return (
        this.reconcileSummary.totalWithdrawn - this.reconcileSummary.totalSpent
      )
    },
    locationName() {
      return this.location ? this.location.locationName : ''
    },
    currencyObj() {
      return this.currency ? this.currency : { isoCode: '', currencyName: '' }
    },
    difference() {
      return round(this.cashOnHand - this.shouldBe, 3)
    }
  }
}
</script>

<style scoped>
h3 {
  display: inline;
}
</style>
