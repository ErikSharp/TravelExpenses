<template>
  <v-select
    :items="locations"
    v-model="location"
    return-object
    box
    background-color="white"
    color="primary"
    label="Location Filter"
  >
    <template slot="selection" slot-scope="data">{{
      getLocationString(data.item)
    }}</template>
    <template slot="item" slot-scope="data">{{
      getLocationString(data.item)
    }}</template>
  </v-select>
</template>

<script>
import sortBy from 'lodash/sortBy'

export default {
  methods: {
    getLocationString(locationObj) {
      if (locationObj.countryName) {
        return `${locationObj.locationName}, ${locationObj.countryName}`
      } else {
        return locationObj.locationName
      }
    }
  },
  computed: {
    locations() {
      let result = sortBy(
        this.$store.state.Location.locations,
        l => l.locationName
      )

      result.unshift({
        locationName: 'All Locations'
      })

      return result
    },
    location: {
      get() {
        return this.$store.state.Location.selectedLocation
      },
      set(val) {
        this.$store
          .dispatch('Location/setSelectedFilterLocation', val)
          .then(() => {
            this.$emit('onChange')
          })
      }
    }
  }
}
</script>

<style scoped>
>>> .v-input__slot {
  margin: 0;
}

>>> .v-text-field__details {
  display: none;
}

>>> .v-input__slot::before {
  border-width: 0 !important;
}

>>> input {
  display: none;
}

>>> .v-select__selections {
  margin-bottom: 8px;
}
</style>