<template>
  <div>
    <h2 class="white--text">Added locations</h2>
    <v-list subheader>
      <v-list-tile v-if="busy">
        <v-list-tile-content>
          <v-progress-circular
            indeterminate
            color="primary"
          ></v-progress-circular>
        </v-list-tile-content>
      </v-list-tile>
      <v-list-tile v-else-if="listItems.length < 1">
        <v-list-tile-content>
          <v-list-tile-title
            >You currently don't have any locations added</v-list-tile-title
          >
        </v-list-tile-content>
      </v-list-tile>
      <template v-for="(item, index) in listItems">
        <v-list-tile @click="edit(item)" :key="item.id">
          <v-list-tile-content>
            <v-list-tile-title v-text="item.locationName"></v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
        <v-divider
          v-if="index + 1 < listItems.length"
          class="my-0"
          :key="item.id + 'div'"
        ></v-divider>
      </template>
    </v-list>
    <hr class="my-4" />
    <v-window touchless v-model="editWindow">
      <v-window-item>
        <add-location @cancel="cancelAdd" />
      </v-window-item>
      <v-window-item>
        <edit-location :location="selectedLocation" @cancel="cancelEdit" />
      </v-window-item>
    </v-window>
  </div>
</template>

<script>
import orderBy from 'lodash/orderBy'
import clone from 'lodash/clone'
import AddLocation from '@/components/setup/location/AddLocation.vue'
import EditLocation from '@/components/setup/location/EditLocation.vue'
import SetupWindows from '@/common/enums/SetupWindows.js'

export default {
  data() {
    return {
      editWindow: 0,
      selectedLocation: {}
    }
  },
  components: {
    AddLocation,
    EditLocation
  },
  methods: {
    edit(item) {
      this.selectedLocation = clone(item)
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
      let locations = this.$store.state.Location.locations
      return orderBy(locations, [c => c.locationName.toLowerCase()])
    },
    busy() {
      return this.$store.state.Location.busy
    }
  }
}
</script>

<style scoped></style>
