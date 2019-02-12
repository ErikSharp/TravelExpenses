<template>
  <v-container>
    <v-window touchless v-model="reconcileWindowId">
      <v-window-item>
        <reconcile-cash-calc />
      </v-window-item>
      <v-window-item>
        <reconcile-summary />
      </v-window-item>
      <v-window-item>
        <reconcile-investigation />
      </v-window-item>
      <v-window-item>
        <button-wrapper :buttonText="buttonText" @buttonClicked="navToSummary">
          <transactions />
        </button-wrapper>
      </v-window-item>
      <v-window-item>
        <button-wrapper :buttonText="buttonText" @buttonClicked="navToSummary">
          <cash-withdrawals />
        </button-wrapper>
      </v-window-item>
      <v-window-item>
        <button-wrapper :buttonText="buttonText" @buttonClicked="navToSummary">
          <reconcile-loss-gain />
        </button-wrapper>
      </v-window-item>
    </v-window>
  </v-container>
</template>

<script>
import ReconcileCashCalc from '@/components/reconcile/ReconcileCashCalc.vue'
import ReconcileSummary from '@/components/reconcile/ReconcileSummary.vue'
import ReconcileInvestigation from '@/components/reconcile/ReconcileInvestigation.vue'
import ButtonWrapper from '@/components/reconcile/ButtonWrapper.vue'
import Transactions from '@/components/transaction/Transactions.vue'
import CashWithdrawals from '@/components/cashWithdrawal/CashWithdrawals.vue'
import ReconcileLossGain from '@/components/reconcile/ReconcileLossGain.vue'
import Windows from '@/common/enums/ReconcileWindows.js'

export default {
  components: {
    ReconcileCashCalc,
    ReconcileSummary,
    ReconcileInvestigation,
    ButtonWrapper,
    Transactions,
    CashWithdrawals,
    ReconcileLossGain
  },
  data() {
    return {
      buttonText: 'Return to Summary'
    }
  },
  methods: {
    navToSummary() {
      this.$store.dispatch('Reconcile/setReconcileWindowId', Windows.summary)
    }
  },
  computed: {
    reconcileWindowId() {
      return this.$store.state.Reconcile.reconcileWindowId
    }
  }
}
</script>

<style scoped></style>
