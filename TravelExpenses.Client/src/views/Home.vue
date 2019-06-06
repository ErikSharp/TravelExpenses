<template>
  <div>
    <home-header />
    <transition name="fade">
      <router-view class="mb-5" :key="$route.fullPath" />
    </transition>
    <v-bottom-nav :active.sync="homeView" :value="true" fixed color="primary">
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
        <span>Queries</span>
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
  computed: {
    homeView: {
      get() {
        return this.$store.state.homeView
      },
      set(newValue) {
        this.$router.push({ name: newValue })
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

button {
  padding: 8px 0 10px 0 !important;
  min-width: 65px !important;
}
</style>
