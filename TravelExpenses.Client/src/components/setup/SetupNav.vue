<template>
  <div>
    <query-card
      icon="add_location"
      title="Locations"
      class="mb-2"
      @click="navLocations"
    >
      Add the locations you will be travelling to
    </query-card>
    <query-card
      icon="collections_bookmark"
      title="Categories"
      class="mb-2"
      @click="navCategories"
    >
      Add categories that will be used to group transactions
    </query-card>
    <query-card icon="vpn_key" title="Keywords" @click="navKeywords">
      Add keywords that can be used to enable querying of transactions
    </query-card>
  </div>
</template>

<script>
import SetupWindow from '@/common/enums/SetupWindows.js'
import QueryCard from '@/components/query/QueryCard.vue'

export default {
  components: {
    QueryCard
  },
  methods: {
    navigate(window) {
      this.$store.dispatch('SetupData/setSetupWindow', window)
    },
    navLocations() {
      this.navigate(SetupWindow.locations)
    },
    navCategories() {
      this.navigate(SetupWindow.categories)
    },
    navKeywords() {
      this.navigate(SetupWindow.keywords)
    }
  }
}
</script>

<style scoped></style>
