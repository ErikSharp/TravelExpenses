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
          <v-card
            light
            class="my-1"
            v-for="(transaction, i) in dateGroup"
            :key="i"
          >
            <v-flex>
              <v-layout align-center justify-start row fill-height>
                <v-flex shrink>
                  <v-avatar
                    size="70"
                    class="mx-2 elevation-5"
                    :class="getColor()"
                  >
                    <v-icon size="45" class="white--text">{{
                      getIcon()
                    }}</v-icon>
                  </v-avatar>
                </v-flex>
                <v-flex>
                  <v-card-text class="white py-1 px-0 border-right">
                    <p>
                      <strong>Title:</strong>
                      {{ transaction.title }}
                    </p>
                    <p>
                      <strong>Category:</strong>
                      {{ getCategoryString(transaction.categoryId) }}
                    </p>
                    <p>
                      <strong>Amount:</strong>
                      {{
                        `${transaction.amount} ${getCurrencyIsoString(
                          transaction.currencyId
                        )}`
                      }}
                    </p>
                    <p style="display: inline">
                      <strong>Keywords:</strong>
                    </p>
                    <v-chip
                      small
                      v-for="(id, i) in transaction.keywordIds"
                      :key="i"
                      >{{ getKeywordName(id) }}</v-chip
                    >
                  </v-card-text>
                </v-flex>
              </v-layout>
            </v-flex>
          </v-card>
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
        <v-btn flat disabled class="primary my-3" @click="addTransaction"
          >Edit</v-btn
        >
        <v-btn flat class="primary my-3" disabled @click="addTransaction"
          >Delete</v-btn
        >
      </v-layout>
    </v-flex>
  </div>
</template>

<script>
import groupBy from 'lodash/groupBy'

const dateOptions = {
  weekday: 'long',
  year: 'numeric',
  month: 'short',
  day: '2-digit'
}

const locale = navigator.language || 'en-US'

export default {
  data() {
    return {
      panel: []
    }
  },
  methods: {
    getColor() {
      let color = Math.floor(Math.random() * 5)
      switch (color) {
        case 0:
          return 'indigo'
        case 1:
          return 'purple'
        case 2:
          return 'green'
        case 3:
          return 'red'
        case 4:
          return 'orange'
      }
    },
    getIcon() {
      let icon = Math.floor(Math.random() * 5)
      switch (icon) {
        case 0:
          return 'fastfood'
        case 1:
          return 'shopping_cart'
        case 2:
          return 'airplanemode_active'
        case 3:
          return 'local_hospital'
        case 4:
          return 'beach_access'
      }
    },
    loadMore() {
      this.$store.dispatch('Transaction/getNextTransactions')
    },
    addTransaction() {
      this.$emit('addTransaction')
    },
    getDateString(date) {
      return new Date(date).toLocaleDateString(locale, dateOptions)
    },
    getCategoryString(id) {
      let category = this.$store.getters['Category/findCategory'](id)
      if (category) {
        return category.categoryName
      }

      return 'unknown'
    },
    getCurrencyIsoString(id) {
      let currency = this.$store.getters['Currency/findCurrency'](id)
      if (currency) {
        return currency.isoCode
      }

      return ''
    },
    getKeywordName(id) {
      let keyword = this.$store.getters['Keyword/findKeyword'](id)
      if (keyword) {
        return keyword.keywordName
      }

      return 'unknown'
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

.v-card p {
  margin: 0;
  height: 22px;
}

.v-chip {
  margin: 0 3px 3px 5px !important;
}

.button-background {
  position: fixed;
  width: 100%;
  bottom: 56px;
  background: rgba(0, 0, 0, 0.8);
}

.v-expansion-panel .v-card {
  border-radius: 10px;
}

.border-right {
  border-radius: 10px;
}

.bottom-spacer {
  height: 79px;
}
</style>
