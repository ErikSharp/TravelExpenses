<template>
  <div>
    <h3>Reconcile Summary</h3>
    <v-btn @click="navToInvestigation">Investigate</v-btn>
  </div>
</template>

<script>
import Windows from '@/common/enums/ReconcileWindows.js'

export default {
  methods: {
    navToInvestigation() {
      this.$store.dispatch(
        'Reconcile/setReconcileWindowId',
        Windows.investigation
      )
    }
  }
}
</script>

<style scoped></style>
