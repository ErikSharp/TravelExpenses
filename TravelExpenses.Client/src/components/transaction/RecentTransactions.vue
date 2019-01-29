<template>
  <div class="mt-5">
    <v-expansion-panel expand>
      <v-expansion-panel-content
        class="primary"
        v-for="(dateGroup, date) in recentTransactions"
        :key="date"
      >
        <div slot="header">{{date}}</div>
        <div style="background: #261136" class="py-1 px-2">
          <v-card class="my-1" v-for="(transaction, i) in dateGroup" :key="i">
            <v-card-text class="white">
              <pre>{{transaction}}</pre>
            </v-card-text>
          </v-card>
        </div>
      </v-expansion-panel-content>
    </v-expansion-panel>
    <v-flex class="button-background" xs12>
      <v-layout justify-center>
        <v-btn class="primary" @click="addTransaction">Add</v-btn>
      </v-layout>
    </v-flex>
  </div>
</template>

<script>
import groupBy from 'lodash/groupBy'

export default {
  created() {
    this.$store.dispatch('Transaction/reloadRecentTransactions')
  },
  methods: {
    addTransaction() {
      this.$emit('addTransaction')
    }
  },
  computed: {
    recentTransactions() {
      return groupBy(
        this.$store.state.Transaction.recentTransactions,
        t => t.transDate
      )
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
