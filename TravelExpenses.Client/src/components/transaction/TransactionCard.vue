<template>
  <v-card
    light
    class="my-1"
    @click="selectTransaction()"
    :class="{
      selected: transactionSelected,
      'py-1': !transaction.keywordIds.length
    }"
  >
    <v-flex>
      <v-layout align-center justify-start row fill-height>
        <v-flex shrink>
          <div id="icons">
            <v-avatar size="70" class="mx-2 elevation-5" :color="getColor()">
              <v-icon size="45" class="white--text">{{ getIcon() }}</v-icon>
            </v-avatar>
            <v-avatar
              v-if="transaction.paidWithCash"
              id="cash-icon"
              class="elevation-5"
              size="25"
              color="green"
            >
              <v-icon size="20" class="white--text">money</v-icon>
            </v-avatar>
          </div>
        </v-flex>
        <v-flex>
          <v-card-text class="white py-1 px-0 border-right">
            <v-layout column>
              <p>
                <strong>Title:</strong>
                {{ transaction.title }}
              </p>
              <p>
                <strong>Location:</strong>
                {{ getLocationString() }}
              </p>
              <p>
                <strong>Category:</strong>
                {{ getCategoryString(transaction.categoryId) }}
              </p>
              <p>
                <strong>Amount:</strong>
                {{
                  `${amountString} ${getCurrencyIsoString(
                    transaction.currencyId
                  )}`
                }}
              </p>
              <div>
                <p v-if="transaction.keywordIds.length" style="display: inline">
                  <strong>Keywords:</strong>
                </p>
                <v-chip
                  small
                  v-for="(id, i) in transaction.keywordIds"
                  :key="i"
                  >{{ getKeywordName(id) }}</v-chip
                >
              </div>
              <v-layout>
                <info-dialog
                  v-if="transaction.memo"
                  color="primary"
                  title="Memo"
                  icon="create"
                >
                  <pre style="font-family: inherit;white-space: pre-wrap">{{
                    transaction.memo
                  }}</pre>
                </info-dialog>
                <v-btn v-if="false" color="primary" small outline>GPS</v-btn>
              </v-layout>
            </v-layout>
          </v-card-text>
        </v-flex>
      </v-layout>
    </v-flex>
  </v-card>
</template>

<script>
import { toLocaleStringWithEndingZero } from '@/common/StringUtilities.js'
import { mapState, mapGetters } from 'vuex'
import InfoDialog from '@/components/common/InfoDialog.vue'

export default {
  components: {
    InfoDialog
  },
  props: {
    transaction: Object
  },
  methods: {
    getIcon() {
      const category = this.findCategory(this.transaction.categoryId)
      if (category) {
        if (category === this.$store.state.Category.lossGainCategory) {
          if (this.transaction.amount > 0) {
            return 'trending_down'
          } else {
            return 'trending_up'
          }
        }
        return category.icon
      } else {
        return 'live_help'
      }
    },
    getColor() {
      const category = this.findCategory(this.transaction.categoryId)

      if (category) {
        if (category === this.$store.state.Category.lossGainCategory) {
          if (this.transaction.amount > 0) {
            return this.$vuetify.theme.error
          } else {
            return this.$vuetify.theme.success
          }
        }

        const HTMLcolor = category.color.toString(16)
        return '#000000'.substring(0, 7 - HTMLcolor.length) + HTMLcolor
      } else {
        return '#000000'
      }
    },
    selectTransaction() {
      this.$store.dispatch(
        'Transaction/setSelectedTransaction',
        this.transaction
      )
    },
    getCategoryString(id) {
      let category = this.$store.state.Category.categories.find(
        c => c.id === id
      )
      if (category) {
        return category.categoryName
      } else if (this.$store.state.Category.lossGainCategory.id === id) {
        return this.$store.state.Category.lossGainCategory.categoryName
      } else {
        return 'unknown'
      }
    },
    getLocationString() {
      const location = this.findLocation(this.transaction.locationId)

      let country = null
      if (location) {
        country = this.findCountry(location.countryId)

        if (country) {
          return `${location.locationName}, ${country.countryName}`
        } else {
          return location.locationName
        }
      } else {
        return 'Unknown'
      }
    },
    getKeywordName(id) {
      let keyword = this.$store.getters['Keyword/findKeyword'](id)
      if (keyword) {
        return keyword.keywordName
      }

      return 'unknown'
    },
    getCurrencyIsoString(id) {
      let currency = this.$store.getters['Currency/findCurrency'](id)
      if (currency) {
        return `${currency.isoCode} - ${currency.currencyName}`
      }

      return ''
    }
  },
  computed: {
    ...mapState('Category', ['lossGainCategory']),
    ...mapGetters('Category', ['findCategory']),
    ...mapGetters('Location', ['findLocation']),
    ...mapGetters('Country', ['findCountry']),
    transactionSelected() {
      return (
        this.$store.state.Transaction.selectedTransaction == this.transaction
      )
    },
    amountString() {
      return toLocaleStringWithEndingZero(Math.abs(this.transaction.amount))
    }
  }
}
</script>

<style scoped>
.v-card p {
  margin: 0;
  overflow: hidden;
}

#icons {
  position: relative;
}

#cash-icon {
  position: absolute;
  top: -8px;
  left: 4px;
  z-index: 0;
}

.v-chip {
  margin: 0 3px 3px 5px !important;
}

.v-expansion-panel .v-card {
  border-radius: 10px;
}

.border-right {
  border-radius: 10px;
}

.selected {
  border: solid var(--v-accent-base) 5px;
}

/* chip text was going over add edit delete buttons */
.v-chip {
  z-index: 0;
}
</style>
