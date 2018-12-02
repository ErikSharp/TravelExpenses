<template>
  <div>
    <v-window v-model="reconcileScreen">
      <v-window-item>
        <reconcile-cash-calc @done="navToSummary"/>
      </v-window-item>
      <v-window-item>
        <reconcile-summary @nextWindow="navToAdjustments"/>
      </v-window-item>
      <v-window-item>
        <reconcile-adjustments :screen="adjustmentsScreen" @returnToSummary="navToSummary"/>
      </v-window-item>
    </v-window>
  </div>
</template>

<script>
import ReconcileCashCalc from '@/components/ReconcileCashCalc.vue'
import ReconcileSummary from '@/components/ReconcileSummary.vue'
import ReconcileAdjustments from '@/components/ReconcileAdjustments.vue'

export default {
  components: {
    ReconcileCashCalc,
    ReconcileSummary,
    ReconcileAdjustments
  },
  data() {
    return {
      reconcileScreen: 0,
      adjustmentsScreen: 0
    }
  },
  methods: {
    navToSummary() {
      this.reconcileScreen = 1
    },
    navToAdjustments(windowId) {
      switch (windowId) {
        case 0:
          this.reconcileScreen = 0
          break
        case 1:
          this.reconcileScreen = 2
          this.adjustmentsScreen = 0
          break
        case 2:
          this.reconcileScreen = 2
          this.adjustmentsScreen = 1
          break
        case 3:
          this.reconcileScreen = 2
          this.adjustmentsScreen = 2
          break
      }
    }
  }
}
</script>

<style scoped>
</style>
