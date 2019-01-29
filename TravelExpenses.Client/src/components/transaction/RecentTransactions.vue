<template>
  <div>
    <h1 class="white--text">Recent Transactions</h1>
    <pre>{{ recentTransactions }}</pre>
    <v-btn @click="addTransaction">Add</v-btn>
  </div>
</template>

<script>
import groupBy from 'lodash/groupBy'

export default {
  created() {
    this.$store.dispatch('Transaction/reloadRecentTransactions')
  },
  methods: {
    addTransaction() {
      this.$emit('addTransaction')
    }
  },
  computed: {
    recentTransactions() {
      return groupBy(
        this.$store.state.Transaction.recentTransactions,
        t => t.transDate
      )
    }
  }
}
</script>

<style scoped>
pre {
  color: white;
}
</style>
