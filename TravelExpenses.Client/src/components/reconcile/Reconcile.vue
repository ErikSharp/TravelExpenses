<template>
  <v-window touchless v-model="reconcileWindowId">
    <v-window-item>
      <reconcile-instructions />
    </v-window-item>
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
      <button-wrapper
        v-if="reconcileWindowId === windows.transactions"
        :buttonText="buttonText"
        @buttonClicked="navToSummary"
      >
        <transactions />
      </button-wrapper>
    </v-window-item>
    <v-window-item>
      <button-wrapper
        v-if="reconcileWindowId === windows.cashWithdrawals"
        :buttonText="buttonText"
        @buttonClicked="navToSummary"
      >
        <cash-withdrawals />
      </button-wrapper>
    </v-window-item>
    <v-window-item>
      <reconcile-loss-gain @returnToSummary="navToSummary" />
    </v-window-item>
  </v-window>
</template>

<script>
import ReconcileCashCalc from '@/components/reconcile/ReconcileCashCalc.vue'
import ReconcileInstructions from '@/components/reconcile/ReconcileInstructions.vue'
import ReconcileSummary from '@/components/reconcile/ReconcileSummary.vue'
import ReconcileInvestigation from '@/components/reconcile/ReconcileInvestigation.vue'
import ButtonWrapper from '@/components/reconcile/ButtonWrapper.vue'
import Transactions from '@/components/transaction/Transactions.vue'
import CashWithdrawals from '@/components/cashWithdrawal/CashWithdrawals.vue'
import ReconcileLossGain from '@/components/reconcile/ReconcileLossGain.vue'
import Windows from '@/common/enums/ReconcileWindows.js'

export default {
  components: {
    ReconcileInstructions,
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
      buttonText: 'Return to Summary',
      windows: Windows
    }
  },
  methods: {
    navToSummary() {
      this.$store.dispatch('Reconcile/setReconcileWindowId', Windows.summary)
    }
  },
  computed: {
    reconcileWindowId() {
      let windowId = this.$store.state.Reconcile.reconcileWindowId

      if (
        windowId === Windows.instructions &&
        !this.$store.state.User.preferences.ShowReconcileInstructions
      ) {
        this.$store.dispatch('Reconcile/setReconcileWindowId', Windows.cashCalc)
        return Windows.cashCalc
      } else {
        return windowId
      }
    }
  }
}
</script>

<style scoped></style>
