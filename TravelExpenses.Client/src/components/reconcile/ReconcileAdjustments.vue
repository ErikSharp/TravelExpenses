<template>
  <div>
    <button-wrapper
      v-if="screen === 0"
      :buttonText="buttonText"
      @buttonClicked="navToSummary"
    >
      <transactions />
    </button-wrapper>
    <button-wrapper
      v-if="screen === 1"
      :buttonText="buttonText"
      @buttonClicked="navToSummary"
    >
      <cash-withdrawals />
    </button-wrapper>
    <button-wrapper
      v-if="screen === 2"
      :buttonText="buttonText"
      @buttonClicked="navToSummary"
    >
      <reconcile-loss-gain />
    </button-wrapper>
  </div>
</template>

<script>
import Transactions from '@/components/Transactions.vue'
import ReconcileLossGain from '@/components/reconcile/ReconcileLossGain.vue'
import CashWithdrawals from '@/components/CashWithdrawals.vue'
import ButtonWrapper from '@/components/reconcile/ButtonWrapper.vue'

export default {
  props: {
    screen: {
      type: Number
    }
  },
  data() {
    return {
      buttonText: 'Return to Summary'
    }
  },
  components: {
    Transactions,
    ReconcileLossGain,
    CashWithdrawals,
    ButtonWrapper
  },
  methods: {
    navToSummary() {
      this.$emit('returnToSummary')
    }
  }
}
</script>

<style scoped></style>
