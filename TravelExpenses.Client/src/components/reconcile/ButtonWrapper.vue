<template>
  <div>
    <slot/>
    <v-btn
      :loading="reconcileBusy"
      class="summary-btn elevation-10"
      small
      fixed
      @click="returnToSummary"
    >{{ buttonText }}</v-btn>
  </div>
</template>

<script>
import { mapState } from 'vuex'

export default {
  props: {
    buttonText: {
      type: String,
      default: 'Cancel'
    }
  },
  methods: {
    returnToSummary() {
      this.$store.dispatch('Reconcile/getReconcileSummary').then(() => {
        if (this.reconcileSummary.totalWithdrawn) {
          this.$emit('buttonClicked')
        } else {
          this.$store.dispatch(
            'showErrorMessage',
            'There are no cash withdrawals to reconcile!'
          )
        }
      })
    }
  },
  computed: {
    ...mapState('Reconcile', ['reconcileBusy', 'reconcileSummary'])
  }
}
</script>

<style scoped>
.summary-btn {
  z-index: 2;
  top: 13px;
  left: 124px;
}
</style>
