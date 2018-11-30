<template>
  <div>
    <home-header></home-header>
    <transition name="fade">
      <router-view :key="$route.fullPath"/>
    </transition>
    <v-bottom-nav :active.sync="homeView" :value="true" fixed shift color="primary">
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
  created() {
    this.$store.dispatch('setHomeView', this.$router.currentRoute.name)
  },
  computed: {
    homeView: {
      get() {
        return this.$store.state.homeView
      },
      set(newValue) {
        return this.$store.dispatch('setHomeView', newValue)
      }
    }
  }
}
</script>

<style scoped lang="scss">
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.25s;
}
.fade-enter-active {
  transition-delay: 0.25s;
}
.fade-enter, .fade-leave-to /* .fade-leave-active below version 2.1.8 */ {
  opacity: 0;
}
</style>
