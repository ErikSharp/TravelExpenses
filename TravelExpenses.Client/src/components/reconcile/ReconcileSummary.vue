<template>
  <div>
    <h3>Reconcile Summary</h3>
    <v-btn @click="nextWindow(0)">Recount Cash</v-btn>
    <v-btn @click="nextWindow(1)">Review Transactions</v-btn>
    <v-btn @click="nextWindow(2)">Add Cash</v-btn>
    <v-btn @click="nextWindow(3)">Make Loss/Gain adjustment</v-btn>
  </div>
</template>

<script>
export default {
  methods: {
    nextWindow(windowId) {
      this.$emit('nextWindow', windowId)
    }
  }
}
</script>

<style scoped>
</style>
