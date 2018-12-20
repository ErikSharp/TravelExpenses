<template>
  <div>
    <h2 class="white--text">Added countries</h2>
    <v-list subheader>
      <v-list-tile v-if="busy">
        <v-list-tile-content>
          <v-progress-circular indeterminate color="primary"></v-progress-circular>
        </v-list-tile-content>
      </v-list-tile>
      <v-list-tile v-else-if="listItems.length < 1">
        <v-list-tile-content>
          <v-list-tile-title>You currently don't have any countries added</v-list-tile-title>
        </v-list-tile-content>
      </v-list-tile>
      <template v-for="(item, index) in listItems">
        <v-list-tile @click="edit(item)" :key="item">
          <v-list-tile-content>
            <v-list-tile-title v-text="item"></v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
        <v-divider v-if="index + 1 < listItems.length" class="my-0" :key="item + 'div'"></v-divider>
      </template>
    </v-list>
    <hr class="my-4">
    <add-country/>
  </div>
</template>

<script>
import sortBy from 'lodash/sortBy'
import AddCountry from '@/components/setup/AddCountry.vue'

export default {
  components: {
    AddCountry
  },
  methods: {
    edit(item) {
      alert(item)
    }
  },
  computed: {
    listItems() {
      let countries = this.$store.state.Country.countries.map(
        c => c.countryName
      )
      return sortBy(countries)
    },
    busy() {
      return this.$store.state.Country.busy
    }
  }
}
</script>

<style scoped>
</style>
