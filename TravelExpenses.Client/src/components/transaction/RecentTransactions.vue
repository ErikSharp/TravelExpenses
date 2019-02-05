<template>
  <div class="mt-5">
    <v-expansion-panel dark v-model="panel" expand>
      <v-expansion-panel-content
        class="primary mt-1"
        v-for="(dateGroup, date) in recentTransactions"
        :key="date"
      >
        <div slot="header">{{ getDateString(date) }}</div>
        <div style="background: #261136" class="py-1 px-2">
          <transaction-card
            v-for="(transaction, i) in dateGroup"
            :key="i"
            :transaction="transaction"
          />
        </div>
      </v-expansion-panel-content>
    </v-expansion-panel>
    <v-layout justify-center>
      <v-btn
        flat
        :loading="busy"
        :disabled="noMoreTransactions"
        class="primary"
        @click="loadMore"
        >Load More</v-btn
      >
    </v-layout>
    <div class="bottom-spacer"></div>
    <v-flex class="button-background" xs12>
      <v-layout justify-center justify-space-between class="mx-5">
        <v-btn flat class="primary my-3" @click="addTransaction">Add</v-btn>
        <v-btn
          flat
          :disabled="!transactionSelected"
          class="primary my-3"
          @click="addTransaction"
          >Edit</v-btn
        >
        <v-btn
          flat
          :disabled="!transactionSelected"
          class="primary my-3"
          @click="addTransaction"
          >Delete</v-btn
        >
      </v-layout>
    </v-flex>
  </div>
</template>

<script>
/* eslint-disable no-console */
import groupBy from 'lodash/groupBy'
import TransactionCard from '@/components/transaction/TransactionCard.vue'

const dateOptions = {
  weekday: 'long',
  year: 'numeric',
  month: 'short',
  day: '2-digit'
}

const locale = navigator.language || 'en-US'

export default {
  components: {
    TransactionCard
  },
  data() {
    return {
      panel: []
    }
  },
  methods: {
    loadMore() {
      this.$store.dispatch('Transaction/getNextTransactions')
    },
    addTransaction() {
      this.$emit('addTransaction')
    },
    getDateString(date) {
      return new Date(date).toLocaleDateString(locale, dateOptions)
    }
  },
  computed: {
    recentTransactions() {
      return groupBy(
        this.$store.state.Transaction.recentTransactions,
        t => t.transDate
      )
    },
    busy() {
      return this.$store.state.Transaction.recentTransactionsBusy
    },
    noMoreTransactions() {
      return this.$store.state.Transaction.noMoreTransactions
    },
    transactionSelected() {
      return !!this.$store.state.Transaction.selectedTransaction.title
    }
  },
  watch: {
    recentTransactions(val) {
      this.panel = new Array(val.length)
      this.panel.fill(false)

      if (this.panel.length) {
        this.panel[0] = true
      }
    }
  }
}
</script>

<style scoped>
.v-expansion-panel {
  margin-top: 56px;
}

.button-background {
  position: fixed;
  width: 100%;
  bottom: 56px;
  background: rgba(0, 0, 0, 0.8);
}

.bottom-spacer {
  height: 79px;
}
</style>
