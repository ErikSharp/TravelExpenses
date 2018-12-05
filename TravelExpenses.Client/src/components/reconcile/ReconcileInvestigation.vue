<template>
  <div>
    <v-btn @click="navigateCashCalc">Recount Cash</v-btn>
    <v-btn @click="navigateTransactions">Review Transactions</v-btn>
    <v-btn @click="navigateCashWithdrawals">Add Cash</v-btn>
    <v-btn @click="navigateLossGain">Make Loss/Gain adjustment</v-btn>
  </div>
</template>

<script>
import Windows from '@/common/enums/ReconcileWindows.js'

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
  }
}
</script>

<style scoped>
</style>
