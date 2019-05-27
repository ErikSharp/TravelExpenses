<template>
  <v-card
    light
    class="my-1"
    @click="selectCashWithdrawal()"
    :class="{
      selected: cashWithdrawalSelected,
      'py-1': !selectCashWithdrawal.memo
    }"
  >
    <v-flex>
      <v-layout align-center justify-start row fill-height>
        <v-flex shrink>
          <v-avatar size="70" class="mx-2 elevation-5 primary">
            <v-icon size="45" class="white--text">money</v-icon>
          </v-avatar>
        </v-flex>
        <v-flex>
          <v-card-text class="white py-1 px-0 border-right">
            <p>
              <strong>Title:</strong>
              {{ cashWithdrawal.title }}
            </p>
            <p>
              <strong>Amount:</strong>
              {{ `${cashWithdrawal.amount.toLocaleString()}` }}
            </p>
            <p>
              <strong>Currency:</strong>
              {{ getCurrencyString(cashWithdrawal.currencyId) }}
            </p>
            <p>
              <strong>Location:</strong>
              {{ getLocationString(cashWithdrawal.locationId) }}
            </p>
            <v-btn
              small
              outline
              class="primary"
              v-if="cashWithdrawal.memo"
              @click.stop="showMemo"
              >Memo</v-btn
            >
          </v-card-text>
        </v-flex>
      </v-layout>
    </v-flex>
  </v-card>
</template>

<script>
export default {
  props: {
    cashWithdrawal: Object
  },
  data() {
    return {
      memoDialog: false
    }
  },
  methods: {
    showMemo() {
      this.$emit('showMemo', this.cashWithdrawal.memo)
    },
    selectCashWithdrawal() {
      this.$store.dispatch(
        'CashWithdrawal/setSelectedCashWithdrawal',
        this.cashWithdrawal
      )
    },
    getCurrencyString(id) {
      let currency = this.$store.getters['Currency/findCurrency'](id)
      if (currency) {
        return `${currency.isoCode} - ${currency.currencyName}`
      }

      return ''
    },
    getLocationString(id) {
      let location = this.$store.getters['Location/findLocation'](id)
      if (location) {
        let country = this.$store.getters['Country/findCountry'](
          location.countryId
        )

        if (country) {
          return `${location.locationName} - ${country.countryName}`
        } else {
          return location.locationName
        }
      }

      return ''
    }
  },
  computed: {
    cashWithdrawalSelected() {
      return (
        this.$store.state.CashWithdrawal.selectedCashWithdrawal ==
        this.cashWithdrawal
      )
    }
  }
}
</script>

<style scoped>
.v-card p {
  margin: 0;
  height: 22px;
  overflow: hidden;
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