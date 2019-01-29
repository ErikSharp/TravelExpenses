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
            <v-card-text class="white">
              <pre>{{ transaction }}</pre>
            </v-card-text>
          </v-card>
        </div>
      </v-expansion-panel-content>
    </v-expansion-panel>
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

.button-background {
  background: rgba(0, 0, 0, 0.8);
}
</style>
