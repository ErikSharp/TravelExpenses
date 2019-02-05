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
        :loading="recentTransactionsBusy"
        :disabled="noMoreTransactions"
        class="primary"
        @click="loadMore"
        >Load More</v-btn
      >
    </v-layout>
    <div class="bottom-spacer"></div>
    <v-flex class="button-background" xs12>
      <v-layout justify-center justify-space-between class="mx-5">
        <v-btn
          flat
          :disabled="saveTransactionBusy"
          class="primary my-3"
          @click="addTransaction"
          >Add</v-btn
        >
        <v-btn
          flat
          :disabled="!transactionSelected || saveTransactionBusy"
          class="primary my-3"
          @click="addTransaction"
          >Edit</v-btn
        >
        <v-dialog
          v-model="deleteDialog"
          :disabled="!transactionSelected || saveTransactionBusy"
          persistent
          max-width="290"
        >
          <v-btn
            slot="activator"
            flat
            :disabled="!transactionSelected || saveTransactionBusy"
            class="primary my-3"
            >Delete</v-btn
          >
          <v-card>
            <v-avatar size="70" class="mx-2 elevation-5 red">
              <v-icon size="45" class="white--text">warning</v-icon>
            </v-avatar>
            <v-card-title class="headline">Delete Transaction</v-card-title>
            <v-card-text
              >There is no way to undo this procedure. Do you wish to
              proceed?</v-card-text
            >
            <v-card-actions>
              <v-layout justify-space-around>
                <v-btn color="red" dark @click="deleteTransaction">YES</v-btn>
                <v-btn color="primary" @click="deleteDialog = false">NO</v-btn>
              </v-layout>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-layout>
    </v-flex>
  </div>
</template>

<script>
/* eslint-disable no-console */
import groupBy from 'lodash/groupBy'
import TransactionCard from '@/components/transaction/TransactionCard.vue'
import { mapState } from 'vuex'

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
      panel: [],
      deleteDialog: false
    }
  },
  methods: {
    loadMore() {
      this.$store.dispatch('Transaction/getNextTransactions')
    },
    addTransaction() {
      this.$emit('addTransaction')
    },
    deleteTransaction() {
      this.deleteDialog = false
      this.$store.dispatch('Transaction/deleteSelectedTransaction')
    },
    getDateString(date) {
      return new Date(date).toLocaleDateString(locale, dateOptions)
    }
  },
  computed: {
    ...mapState('Transaction', [
      'recentTransactionsBusy',
      'noMoreTransactions',
      'saveTransactionBusy'
    ]),
    recentTransactions() {
      return groupBy(
        this.$store.state.Transaction.recentTransactions,
        t => t.transDate
      )
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
