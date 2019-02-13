<template>
  <div>
    <div
      class="helper-text"
      v-if="!Object.keys(recentTransactions).length && !recentTransactionsBusy"
    >
      <h1 class="text-xs-center white--text">No transactions</h1>
      <p class="text-xs-center white--text">
        Press the Add button below to create your first.
      </p>
    </div>
    <v-layout
      v-show="!Object.keys(recentTransactions).length && recentTransactionsBusy"
      justify-center
    >
      <v-progress-circular
        class="mt-5"
        size="70"
        width="7"
        color="secondary"
        indeterminate
      ></v-progress-circular>
    </v-layout>
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
        v-show="Object.keys(recentTransactions).length"
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
      <v-flex xs12 sm10 offset-sm1>
        <v-layout justify-center justify-space-between>
          <v-btn flat class="primary my-3" @click="addTransaction">
            <v-icon dark>add</v-icon>Add
          </v-btn>
          <v-btn
            flat
            :disabled="!transactionSelected"
            class="primary my-3"
            @click="editTransaction"
          >
            <v-icon dark>edit</v-icon>Edit
          </v-btn>
          <v-dialog
            v-model="deleteDialog"
            :disabled="!transactionSelected"
            persistent
            max-width="290"
          >
            <v-btn
              slot="activator"
              flat
              :disabled="!transactionSelected"
              class="primary my-3"
            >
              <v-icon dark>delete</v-icon>Delete
            </v-btn>
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
                  <v-btn
                    color="red"
                    :loading="saveTransactionBusy"
                    dark
                    @click="deleteTransaction"
                    >YES</v-btn
                  >
                  <v-btn
                    color="primary"
                    :disabled="saveTransactionBusy"
                    @click="deleteDialog = false"
                    >NO</v-btn
                  >
                </v-layout>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-layout>
      </v-flex>
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
  mounted() {
    this.$store.dispatch('Transaction/reloadRecentTransactions')
  },
  data() {
    return {
      panel: [true],
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
    editTransaction() {
      this.$emit('editTransaction')
    },
    deleteTransaction() {
      this.$store.dispatch('Transaction/deleteSelectedTransaction', () => {
        this.deleteDialog = false
      })
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
      this.panel = new Array(Object.keys(val).length)
      this.panel.fill(false)

      if (this.panel.length) {
        this.panel[0] = true
      }
    }
  }
}
</script>

<style scoped>
.button-background {
  position: fixed;
  width: 100%;
  bottom: 56px;
  background: rgba(0, 0, 0, 0.8);
}

.bottom-spacer {
  height: 79px;
}

.helper-text {
  margin-top: 75px;
}
</style>
