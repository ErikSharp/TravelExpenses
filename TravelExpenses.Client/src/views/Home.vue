<template>
  <div>
    <home-header></home-header>
    <router-view :key="$route.fullPath"/>
    <v-bottom-nav
      :active.sync="bottomNav"
      @update:active="navigate"
      :value="true"
      fixed
      shift
      color="primary"
    >
      <v-btn dark value="transactions">
        <span>Home</span>
        <v-icon>home</v-icon>
      </v-btn>

      <v-btn dark value="reconcile">
        <span>Reconcile</span>
        <v-icon>done</v-icon>
      </v-btn>

      <v-btn dark value="cashWithdrawals">
        <span>Add Cash</span>
        <v-icon>money</v-icon>
      </v-btn>

      <v-btn dark value="queries">
        <span>Query</span>
        <v-icon>bar_chart</v-icon>
      </v-btn>

      <v-btn dark value="setup">
        <span>Setup</span>
        <v-icon>settings</v-icon>
      </v-btn>
    </v-bottom-nav>
  </div>
</template>

<script>
// @ is an alias to /src
import HomeHeader from '@/components/HomeHeader.vue'

export default {
  name: 'home',
  components: {
    HomeHeader
  },
  data() {
    return {
      homeWindow: 0,
      bottomNav: 'transactions'
    }
  },
  methods: {
    navigate() {
      this.$router.push({ name: this.bottomNav })
    }
  }
}
</script>
