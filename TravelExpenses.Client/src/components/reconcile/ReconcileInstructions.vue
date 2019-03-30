<template>
  <v-container>
    <v-card>
      <v-card-title primary-title class="pt-3 pb-1">
        <v-avatar color="primary mr-2 mb-2">
          <v-icon class="white--text">done</v-icon>
        </v-avatar>
        <h3 class="headline">Cash Reconciliation</h3>
        <p>
          Foreign currencies can be hard to keep track of as they are sometimes
          in denominations that are different sizes to your own.
        </p>
        <p>This process aims to:</p>
        <ol class="mb-2">
          <li>Highlights if you lost or gained any cash unexpectedly</li>
          <li>
            Confirms that all transactions have been entered and that no cash
            witdrawals have been forgotten
          </li>
        </ol>
        <p class="mb-0">
          If there is a discreptancy with the total, it can be noted to balance
          the amounts and complete reconciliation process.
        </p>
      </v-card-title>
      <v-checkbox
        class="mt-0 ml-3"
        color="primary"
        v-model="doNotShowAgain"
        label="Do not show again"
        hide-details
      >
      </v-checkbox>
      <v-card-actions class="pa-0">
        <v-layout justify-center>
          <v-btn class="ma-2" color="primary" @click="next">Next</v-btn>
        </v-layout>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script>
import clone from 'lodash/clone'
import Windows from '@/common/enums/ReconcileWindows.js'

export default {
  methods: {
    next() {
      this.$store.dispatch('Reconcile/setReconcileWindowId', Windows.cashCalc)
    }
  },
  computed: {
    doNotShowAgain: {
      get() {
        return !this.$store.state.User.preferences.ShowReconcileInstructions
      },
      set(val) {
        this.$store.dispatch('User/setShowReconcileInstructions', !val)
      }
    }
  }
}
</script>

<style scoped>
</style>