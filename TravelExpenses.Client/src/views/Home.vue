<template>
  <div>
    <v-window v-model="homeWindow">
      <v-window-item>
        <transactions></transactions>
      </v-window-item>
    </v-window>
    <v-bottom-nav :active.sync="bottomNav" :value="true" absolute color="transparent">
      <v-btn color="primary" flat value="home">
        <span>Home</span>
        <v-icon>home</v-icon>
      </v-btn>

      <v-btn color="primary" flat value="reconcile">
        <span>Reconcile</span>
        <v-icon>done</v-icon>
      </v-btn>

      <v-btn color="primary" flat value="addCash">
        <span>Add Cash</span>
        <v-icon>money</v-icon>
      </v-btn>

      <v-btn color="primary" flat value="query">
        <span>Query</span>
        <v-icon>bar_chart</v-icon>
      </v-btn>

      <v-btn color="primary" flat value="profile">
        <span>Profile</span>
        <v-icon>person</v-icon>
      </v-btn>
    </v-bottom-nav>
  </div>
</template>

<script>
// @ is an alias to /src
import Transactions from '@/components/Transactions.vue'

export default {
  name: 'home',
  components: {
    Transactions
  },
  data() {
    return {
      homeWindow: 0,
      bottomNav: 'recent'
    }
  }
}
</script>
