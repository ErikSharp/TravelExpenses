<template>
  <div>
    <h2 class="white--text">Edit location</h2>
    <v-text-field
      v-model.trim="updatedLocation.locationName"
      :error-messages="locationErrors"
      label="Edit name"
      box
      background-color="white"
      color="primary"
      @input="$v.updatedLocation.$touch()"
      @blur="$v.updatedLocation.$touch()"
    ></v-text-field>
    <v-autocomplete
      :items="countries"
      v-model="updatedCountry"
      :filter="filterCountry"
      return-object
      :error-messages="countryErrors"
      box
      background-color="white"
      color="primary"
      label="Select country"
      @input="$v.updatedCountry.$touch()"
      @blur="$v.updatedCountry.$touch()"
    >
      <template slot="selection" slot-scope="data">{{ data.item.countryName }}</template>
      <template slot="item" slot-scope="data">{{ data.item.countryName }}</template>
    </v-autocomplete>
    <v-flex xs8 offset-xs2>
      <v-layout row justify-space-around>
        <v-btn dark color="primary" :disabled="$v.$invalid" :loading="busy" @click="edit">Edit</v-btn>
        <v-btn dark color="primary" @click="cancel">Cancel</v-btn>
      </v-layout>
    </v-flex>
  </div>
</template>

<script>
/* eslint-disable no-console */
import { required, minLength, maxLength } from 'vuelidate/lib/validators'
import sortBy from 'lodash/sortBy'

const locationMustBeUnique = (value, vm) => {
  // let locationNameNotChanged =
  //   vm.location.locationName === vm.updatedLocation.locationName

  // if (locationNameNotChanged) {
  //   return true
  // }

  let itemsLowered = vm.items.map(i => i.locationName.toLowerCase())
  let valueIsUnique = itemsLowered.indexOf(value.locationName.toLowerCase()) < 0

  return valueIsUnique
}

// const somethingMustHaveChanged = (value, vm) => {
//   console.log('checking')
//   console.log(vm)

//   let locationNameChanged =
//     vm.location.locationName !== vm.updatedLocation.locationName

//   let countryChanged = vm.location.countryId !== vm.updatedCountry.id

//   return locationNameChanged || countryChanged
// }

export default {
  props: {
    location: Object
  },
  validations() {
    const result = {
      updatedLocation: {
        locationName: {
          required,
          minLength: minLength(3),
          maxLength: maxLength(255)
        },
        locationMustBeUnique
      },
      updatedCountry: {
        required
      }
    }

    return result
  },
  data() {
    return {
      updatedLocation: {},
      updatedCountry: {}
    }
  },
  methods: {
    cancel() {
      this.$emit('cancel')
    },
    edit() {
      let location = {
        id: this.location.id,
        locationName: this.updatedLocation.locationName,
        countryId: this.updatedCountry.id
      }

      this.$store
        .dispatch('Location/editLocation', location)
        .then(() => this.$emit('cancel'))
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
    // country: {
    //   get() {
    //     // if (this.newCountry) {

    //     // }
    //     return {
    //       id: this.newCountryId ? this.newCountryId : this.location.countryId,
    //       countryName: this.location.countryName
    //     }
    //   },
    //   set(value) {
    //     this.newCountryId = value ? value.id : 0
    //   }
    // },
    locationErrors() {
      const errors = []

      if (!this.$v.updatedLocation.$dirty) return errors

      !this.$v.updatedLocation.locationName.maxLength &&
        errors.push(
          `The location can be a maximum of ${
            this.$v.updatedLocation.locationName.$params.maxLength.max
          } characters`
        )
      !this.$v.updatedLocation.locationName.minLength &&
        errors.push(
          `The location must be a minimum of ${
            this.$v.updatedLocation.locationName.$params.minLength.min
          } characters`
        )
      !this.$v.updatedLocation.locationMustBeUnique &&
        errors.push('The location must be unique')

      !this.$v.updatedLocation.locationName.required &&
        errors.push('A location is required')
      return errors
    },
    countryErrors() {
      const errors = []

      if (!this.$v.updatedCountry.$dirty) return errors

      !this.$v.updatedCountry.required && errors.push('A country is required')
      return errors
    },
    items() {
      return this.$store.state.Location.locations
    },
    busy() {
      return this.$store.state.Location.editLocationBusy
    },
    countries() {
      return sortBy(this.$store.state.Country.countries, c => c.countryName)
    }
  },
  watch: {
    location() {
      this.$v.$reset()
      this.updatedLocation = {
        id: this.location.id,
        locationName: this.location.locationName
      }
      this.updatedCountry = {
        id: this.location.countryId,
        countryName: this.location.countryName
      }
    }
  }
}
</script>

<style scoped>
</style>
