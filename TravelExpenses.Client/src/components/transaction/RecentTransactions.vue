<template>
  <div>
    <div
      class="helper-text"
      v-if="!Object.keys(transactions).length && !transactionsBusy"
    >
      <h1 class="text-xs-center white--text">No Transactions</h1>
      <p class="text-xs-center white--text">
        Press the Add button below to create your first.
      </p>
    </div>
    <v-layout
      v-show="!Object.keys(transactions).length && transactionsBusy"
      justify-center
    >
      <v-progress-circular
        class="mt-5"
        size="70"
        width="7"
        color="accent"
        indeterminate
      ></v-progress-circular>
    </v-layout>
    <v-expansion-panel dark v-model="panels" expand>
      <v-expansion-panel-content
        class="primary mt-1"
        v-for="(dateGroup, date) in transactions"
        :key="date"
      >
        <div slot="header">{{ getDateString(date) }}</div>
        <div style="background: var(--v-secondary-base)" class="py-1 px-2">
          <transaction-card
            v-for="(transaction, i) in dateGroup"
            :key="i"
            :transaction="transaction"
          />
        </div>
      </v-expansion-panel-content>
    </v-expansion-panel>
    <div class="bottom-spacer"></div>
    <v-flex class="button-background" xs12 v-show="!transactionsBusy">
      <div class="text-xs-center my-1">
        <v-pagination v-model="page" :length="pageCount"></v-pagination>
      </div>
      <v-flex xs12 sm10 offset-sm1>
        <v-layout justify-center justify-space-between>
          <v-btn flat class="primary mb-2 mt-0" @click="addTransaction">
            <v-icon dark>add</v-icon>Add
          </v-btn>
          <v-btn
            flat
            :disabled="!transactionSelected || lossGainSelected"
            class="primary mb-2 mt-0"
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
              class="primary mb-2 mt-0"
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
import { mapState, mapGetters } from 'vuex'

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
      panels: [true],
      deleteDialog: false
    }
  },
  methods: {
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
      'transactionsBusy',
      'saveTransactionBusy',
      'selectedTransaction',
      'totalRecords',
      'pageSize'
    ]),
    ...mapGetters('Transaction', ['pageCount']),
    ...mapState('Category', ['lossGainCategory']),
    page: {
      get() {
        return this.$store.state.Transaction.page
      },
      set(val) {
        this.$store.dispatch('Transaction/getTransactions', val)
      }
    },
    transactions() {
      return groupBy(
        this.$store.state.Transaction.transactions,
        t => t.transDate
      )
    },
    lossGainSelected() {
      return (
        this.transactionSelected &&
        this.selectedTransaction.categoryId === this.lossGainCategory.id
      )
    },
    transactionSelected() {
      return !!this.selectedTransaction.title
    }
  },
  watch: {
    transactions(val) {
      this.panels = new Array(Object.keys(val).length)
      this.panels.fill(false)

      if (this.panels.length) {
        this.panels[0] = true
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

.transactions:first-child {
  margin-top: 0;
}

.bottom-spacer {
  height: 100px;
}

.helper-text {
  margin-top: 75px;
}

>>> .v-pagination__more {
  color: white !important;
}
</style>
