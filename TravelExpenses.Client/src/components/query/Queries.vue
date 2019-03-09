<template>
  <v-container>
    <v-window touchless v-model="queryWindow">
      <v-window-item>
        <query-nav />
      </v-window-item>
      <v-window-item>
        <locations-total-comparison />
      </v-window-item>
      <v-window-item>
        <locations-categories-comparison />
      </v-window-item>
    </v-window>
  </v-container>
</template>

<script>
import QueryNav from '@/components/query/QueryNav.vue'
import LocationsCategoriesComparison from '@/components/query/LocationsCategoriesComparison.vue'
import LocationsTotalComparison from '@/components/query/LocationsTotalComparison.vue'

export default {
  components: {
    QueryNav,
    LocationsCategoriesComparison,
    LocationsTotalComparison
  },
  computed: {
    queryWindow: {
      get() {
        return this.$store.state.Query.queryWindow
      },
      set(val) {
        this.$store.dispatch('Query/setQueryWindow', val)
      }
    }
  }
}
</script>

<style scoped></style>
