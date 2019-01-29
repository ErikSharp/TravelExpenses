<template>
  <div class="mt-5">
    <v-expansion-panel v-model="panel" expand>
      <v-expansion-panel-content
        class="primary mt-1"
        v-for="(dateGroup, date) in recentTransactions"
        :key="date"
      >
        <div class="white--text" slot="header">{{ getDateString(date) }}</div>
        <div style="background: #261136" class="py-1 px-2">
          <v-card class="my-1" v-for="(transaction, i) in dateGroup" :key="i">
            <v-card-text class="white pa-2">
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
              >{{ getKeywordName(id) }}</v-chip>
            </v-card-text>
          </v-card>
        </div>
      </v-expansion-panel-content>
    </v-expansion-panel>
    <div class="bottom-spacer"></div>
    <v-flex class="button-background" xs12>
      <v-layout justify-center>
        <v-btn class="primary my-3" @click="addTransaction">Add</v-btn>
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
  created() {
    this.$store.dispatch('Transaction/reloadRecentTransactions')
  },
  data() {
    return {
      panel: []
    }
  },
  methods: {
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

.bottom-spacer {
  height: 79px;
}
</style>
