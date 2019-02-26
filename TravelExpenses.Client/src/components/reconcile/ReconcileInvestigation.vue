<template>
  <v-container class="pt-0">
    <h2 class="white--text mb-1">Investigate</h2>
    <v-layout column>
      <v-flex class="order-xs1">
        <v-card>
          <v-card-text class="pb-0">{{
            `${resultString}. Did you count your cash correctly?`
          }}</v-card-text>
          <v-btn class="ml-3 mb-3" color="primary" @click="navigateCashCalc"
            >Recount Cash</v-btn
          >
        </v-card>
      </v-flex>
      <v-flex :class="{ 'order-xs3': haveNetGain, 'order-xs2': !haveNetGain }">
        <v-card class="mt-2">
          <v-card-text class="pb-0">
            {{
              `${
                haveNetGain
                  ? 'D'
                  : 'As y' +
                    resultString.substring(1, resultString.length) +
                    ', d'
              }o you need to add additional transactions?`
            }}
          </v-card-text>
          <v-btn class="ml-3 mb-3" color="primary" @click="navigateTransactions"
            >Review Transactions</v-btn
          >
        </v-card>
      </v-flex>
      <v-flex :class="{ 'order-xs2': haveNetGain, 'order-xs3': !haveNetGain }">
        <v-card class="mt-2">
          <v-card-text
            class="pb-0"
            v-html="
              `Have you forgotten to add a cash withdrawal?${
                haveNetGain
                  ? ''
                  : ' <i>This is not likely as you are missing cash that you should have.</i>'
              }`
            "
          ></v-card-text>
          <v-btn
            class="ml-3 mb-3"
            color="primary"
            @click="navigateCashWithdrawals"
            >Add Cash</v-btn
          >
        </v-card>
      </v-flex>
      <v-flex class="order-xs4">
        <v-card class="mt-2">
          <v-card-text class="pb-0">
            {{
              `I have counted the correct amount of cash; reviewed my transactions and
            do not need to add any cash withdrawals. I need to record my ${
              haveNetGain ? 'gain' : 'loss'
            } as an
            adjustment.`
            }}
          </v-card-text>
          <v-btn class="ml-3 mb-3" color="primary" @click="navigateLossGain"
            >Make Loss/Gain adjustment</v-btn
          >
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import Windows from '@/common/enums/ReconcileWindows.js'
import { mapGetters } from 'vuex'

export default {
  methods: {
    navigateCashCalc() {
      this.$store.dispatch('Reconcile/setReconcileWindowId', Windows.cashCalc)
    },
    navigateTransactions() {
      this.$store.dispatch(
        'Reconcile/setReconcileWindowId',
        Windows.transactions
      )
    },
    navigateCashWithdrawals() {
      this.$store.dispatch(
        'Reconcile/setReconcileWindowId',
        Windows.cashWithdrawals
      )
    },
    navigateLossGain() {
      this.$store.dispatch('Reconcile/setReconcileWindowId', Windows.lossGain)
    }
  },
  computed: {
    ...mapGetters('Reconcile', ['resultString', 'haveNetGain'])
  }
}
</script>

<style scoped></style>
