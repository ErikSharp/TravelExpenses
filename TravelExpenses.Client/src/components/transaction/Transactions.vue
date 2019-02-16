<template>
  <div>
    <v-window touchless v-model="transactionWindow">
      <v-window-item>
        <new-transaction edit @done="returnToRecent"></new-transaction>
      </v-window-item>
      <v-window-item>
        <recent-transactions
          @addTransaction="addTransaction"
          @editTransaction="editTransaction()"
        ></recent-transactions>
      </v-window-item>
      <v-window-item>
        <new-transaction @done="returnToRecent"></new-transaction>
      </v-window-item>
    </v-window>
  </div>
</template>

<script>
import RecentTransactions from '@/components/transaction/RecentTransactions.vue'
import NewTransaction from '@/components/transaction/NewTransaction.vue'

export default {
  components: {
    RecentTransactions,
    NewTransaction
  },
  data() {
    return {
      transactionWindow: 1,
      transactionToEdit: {}
    }
  },
  methods: {
    addTransaction() {
      this.transactionWindow = 2
      this.$store.dispatch('setTitle', 'Add Transaction', { root: true })
      this.scrollToTop()
    },
    editTransaction() {
      this.transactionWindow = 0
      this.$store.dispatch('setTitle', 'Edit Transaction', { root: true })
      this.scrollToTop()
    },
    returnToRecent() {
      this.transactionWindow = 1
      this.$store.dispatch('setTitle', 'Transactions', { root: true })
      this.scrollToTop()
    },
    scrollToTop() {
      this.$vuetify.goTo(0, {
        duration: 300,
        offset: 0,
        easing: 'easeInOutCubic'
      })
    }
  }
}
</script>

<style scoped></style>
