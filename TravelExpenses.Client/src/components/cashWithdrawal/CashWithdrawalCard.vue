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
            <info-dialog
              v-if="cashWithdrawal.memo"
              color="primary"
              title="Memo"
              icon="create"
            >
              <pre style="font-family: inherit;white-space: pre-wrap">{{
                cashWithdrawal.memo
              }}</pre>
            </info-dialog>
          </v-card-text>
        </v-flex>
      </v-layout>
    </v-flex>
  </v-card>
</template>

<script>
import InfoDialog from '@/components/common/InfoDialog.vue'

export default {
  components: {
    InfoDialog
  },
  props: {
    cashWithdrawal: Object
  },
  data() {
    return {
      memoDialog: false
    }
  },
  methods: {
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