<template>
  <div>
    <h1>Transactions</h1>
    <v-window v-model="transactionWindow">
      <v-window-item>
        <recent-transactions @addTransaction="addTransaction"></recent-transactions>
      </v-window-item>
      <v-window-item>
        <new-transaction @done="returnToRecent"></new-transaction>
      </v-window-item>
    </v-window>
  </div>
</template>

<script>
import RecentTransactions from '@/components/RecentTransactions.vue'
import NewTransaction from '@/components/NewTransaction.vue'

export default {
  components: {
    RecentTransactions,
    NewTransaction
  },
  data() {
    return {
      transactionWindow: 0
    }
  },
  methods: {
    addTransaction() {
      this.transactionWindow = 1
    },
    returnToRecent() {
      this.transactionWindow = 0
    }
  }
}
</script>

<style scoped>
</style>
