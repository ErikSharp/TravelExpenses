<template>
  <div>
    <h3>Cash Calculator</h3>
    <v-btn @click="navToSummary">Done</v-btn>
  </div>
</template>

<script>
import Windows from '@/common/enums/ReconcileWindows.js'

export default {
  methods: {
    navToSummary() {
      this.$store.dispatch('Reconcile/setReconcileWindowId', Windows.summary)
    }
  }
}
</script>

<style scoped>
</style>
