<template>
  <div>
    <home-header></home-header>
    <v-window v-model="homeWindow">
      <v-window-item>
        <transactions></transactions>
      </v-window-item>
      <v-window-item>
        <reconcile></reconcile>
      </v-window-item>
      <v-window-item>
        <cash-withdrawals></cash-withdrawals>
      </v-window-item>
      <v-window-item>
        <queries></queries>
      </v-window-item>
      <v-window-item>
        <settings></settings>
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

      <v-btn color="primary" flat value="queries">
        <span>Query</span>
        <v-icon>bar_chart</v-icon>
      </v-btn>

      <v-btn color="primary" flat value="settings">
        <span>Settings</span>
        <v-icon>setting</v-icon>
      </v-btn>
    </v-bottom-nav>
  </div>
</template>

<script>
// @ is an alias to /src
import HomeHeader from '@/components/HomeHeader.vue'
import Transactions from '@/components/Transactions.vue'
import Reconcile from '@/components/Reconcile.vue'
import CashWithdrawals from '@/components/CashWithdrawals.vue'
import Queries from '@/components/Queries.vue'
import Settings from '@/components/Settings.vue'

export default {
  name: 'home',
  components: {
    HomeHeader,
    Transactions,
    Reconcile,
    CashWithdrawals,
    Queries,
    Settings
  },
  data() {
    return {
      homeWindow: 0,
      bottomNav: 'recent'
    }
  }
}
</script>
