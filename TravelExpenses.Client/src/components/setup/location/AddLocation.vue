<template>
  <div>
    <h2 class="white--text">Add additional location</h2>
    <v-text-field
      v-model.trim="location"
      :error-messages="locationErrors"
      label="Enter location name"
      box
      background-color="white"
      color="primary"
      @input="$v.location.$touch()"
      @blur="$v.location.$touch()"
    ></v-text-field>
    <v-autocomplete
      :items="countries"
      v-model="country"
      :filter="filterCountry"
      return-object
      :error-messages="countryErrors"
      box
      background-color="white"
      color="primary"
      label="Select country"
      @input="$v.country.$touch()"
      @blur="$v.country.$touch()"
    >
      <template slot="selection" slot-scope="data">{{ data.item.countryName }}</template>
      <template slot="item" slot-scope="data">{{ data.item.countryName }}</template>
    </v-autocomplete>
    <v-flex xs8 offset-xs2>
      <v-layout row justify-space-around>
        <v-btn dark color="primary" :disabled="$v.$invalid" :loading="busy" @click="add">Add</v-btn>
        <v-btn dark color="primary" @click="cancel">Cancel</v-btn>
      </v-layout>
    </v-flex>
  </div>
</template>

<script>
import { required, minLength, maxLength } from 'vuelidate/lib/validators'
import sortBy from 'lodash/sortBy'

const locationMustBeUnique = (value, vm) => {
  let itemsLowered = vm.items.map(i => i.locationName.toLowerCase())
  return itemsLowered.indexOf(value.toLowerCase()) < 0
}

export default {
  validations() {
    const result = {
      location: {
        required,
        minLength: minLength(3),
        maxLength: maxLength(255),
        locationMustBeUnique
      },
      country: {
        required
      }
    }

    return result
  },
  data() {
    return {
      location: '',
      country: {}
    }
  },
  methods: {
    cancel() {
      this.$emit('cancel')
    },
    add() {
      this.$store.dispatch('Location/addLocation', {
        locationName: this.location,
        countryId: this.country.id
      })
    },
    filterCountry(item, queryText) {
      if (queryText.trim() === '') {
        return true
      } else {
        return (
          item.countryName.toLowerCase().indexOf(queryText.toLowerCase()) > -1
        )
      }
    }
  },
  computed: {
    locationErrors() {
      const errors = []

      if (!this.$v.location.$dirty) return errors

      !this.$v.location.maxLength &&
        errors.push(
          `The location can be a maximum of ${
            this.$v.location.$params.maxLength.max
          } characters`
        )
      !this.$v.location.minLength &&
        errors.push(
          `The location must be a minimum of ${
            this.$v.location.$params.minLength.min
          } characters`
        )
      !this.$v.location.locationMustBeUnique &&
        errors.push('The location must be unique')

      !this.$v.location.required && errors.push('A location is required')
      return errors
    },
    countryErrors() {
      const errors = []

      if (!this.$v.country.$dirty) return errors

      !this.$v.country.required && errors.push('A country is required')
      return errors
    },
    items() {
      return this.$store.state.Location.locations
    },
    busy() {
      return this.$store.state.Location.addLocationBusy
    },
    countries() {
      return sortBy(this.$store.state.Country.countries, c => c.countryName)
    }
  },
  watch: {
    items() {
      this.location = ''
      this.country = {}
      this.$v.$reset()
    }
  }
}
</script>

<style scoped>
</style>
