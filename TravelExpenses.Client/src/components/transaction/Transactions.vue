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
      this.setTitle('Add Transaction')
      this.scrollToTop()
    },
    editTransaction() {
      this.transactionWindow = 0
      this.setTitle('Edit Transaction')
      this.scrollToTop()
    },
    returnToRecent() {
      this.transactionWindow = 1
      this.setTitle('Transactions')
      this.scrollToTop()
    },
    setTitle(title) {
      if (this.title !== 'Reconcile') {
        this.$store.dispatch('setTitle', title, { root: true })
      }
    },
    scrollToTop() {
      this.$vuetify.goTo(0, {
        duration: 300,
        offset: 0,
        easing: 'easeInOutCubic'
      })
    }
  },
  computed: {
    title() {
      return this.$store.state.title
    }
  }
}
</script>

<style scoped></style>
