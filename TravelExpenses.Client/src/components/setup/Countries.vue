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
        <v-list-tile @click="edit(item)" :key="item.id">
          <v-list-tile-content>
            <v-list-tile-title v-text="item.countryName"></v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
        <v-divider v-if="index + 1 < listItems.length" class="my-0" :key="item.id + 'div'"></v-divider>
      </template>
    </v-list>
    <hr class="my-4">
    <v-window v-model="editWindow">
      <v-window-item>
        <add-country @cancel="cancelAdd"/>
      </v-window-item>
      <v-window-item>
        <edit-country :country="selectedCountry" @cancel="cancelEdit"/>
      </v-window-item>
    </v-window>
  </div>
</template>

<script>
import orderBy from 'lodash/orderBy'
import clone from 'lodash/clone'
import AddCountry from '@/components/setup/AddCountry.vue'
import EditCountry from '@/components/setup/EditCountry.vue'
import SetupWindows from '@/common/enums/SetupWindows.js'

export default {
  data() {
    return {
      editWindow: 0,
      selectedCountry: {}
    }
  },
  components: {
    AddCountry,
    EditCountry
  },
  methods: {
    edit(item) {
      this.selectedCountry = clone(item)
      this.editWindow = 1
    },
    cancelAdd() {
      this.$store.dispatch('SetupData/setSetupWindow', SetupWindows.navigation)
    },
    cancelEdit() {
      this.editWindow = 0
    }
  },
  computed: {
    listItems() {
      let countries = this.$store.state.Country.countries
      return orderBy(countries, [c => c.countryName.toLowerCase()])
    },
    busy() {
      return this.$store.state.Country.busy
    }
  }
}
</script>

<style scoped>
</style>
