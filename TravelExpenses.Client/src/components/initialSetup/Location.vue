<template>
  <v-card class="pa-3">
    <v-flex xs12 class="mb-3">
      <v-layout row align-center>
        <v-avatar size="55" class="mr-3 elevation-4">
          <v-icon large class="primary white--text">add_location</v-icon>
        </v-avatar>
        <h1>Country and Location</h1>
      </v-layout>
    </v-flex>
    <p>Let's begin by adding the country and location you are travelling to.</p>
    <p>
      <strong>e.g.</strong> I'm travelling to the USA and the location I'm going to is New York.
    </p>
    <v-autocomplete
      :items="countries"
      v-model="selectedCountry"
      :filter="filterCountry"
      return-object
      :error-messages="countryErrors"
      box
      background-color="white"
      color="primary"
      label="Select a country"
      @input="$v.selectedCountry.$touch()"
      @blur="$v.selectedCountry.$touch()"
    >
      <template slot="selection" slot-scope="data">{{ data.item.countryName }}</template>
      <template slot="item" slot-scope="data">{{ data.item.countryName }}</template>
    </v-autocomplete>
    <v-text-field
      v-model.trim="location"
      :error-messages="locationErrors"
      label="Enter a location"
      box
      background-color="white"
      color="primary"
      @input="$v.location.$touch()"
      @blur="$v.location.$touch()"
    ></v-text-field>
    <p>Locations provide a more detailed way to query within a specific country.</p>
    <p>
      <strong>Note:</strong> You will have the ability to add additional location by using the Setup menu with the application.
    </p>
    <v-flex xs12>
      <v-layout row justify-center>
        <v-btn color="primary" :loading="busy" :disabled="$v.$invalid" @click="next">NEXT</v-btn>
      </v-layout>
    </v-flex>
  </v-card>
</template>

<script>
import Windows from '@/common/enums/InitialSetupWindows.js'
import sortBy from 'lodash/sortBy'
import { required, minLength, maxLength } from 'vuelidate/lib/validators'

export default {
  created() {
    this.$store.dispatch('Country/load')
  },
  data() {
    return {
      selectedCountry: {},
      location: ''
    }
  },
  validations() {
    const result = {
      selectedCountry: {
        required
      },
      location: {
        required,
        minLength: minLength(3),
        maxLength: maxLength(255)
      }
    }

    return result
  },
  methods: {
    next() {
      this.$store
        .dispatch('Location/addLocation', {
          locationName: this.location,
          countryId: this.selectedCountry.id
        })
        .then(() => {
          this.$store.dispatch('InitialSetup/setWindow', this.getNextWindow())
        })
    },
    getNextWindow() {
      if (!this.$store.state.InitialSetup.baseData.hasCategory) {
        return Windows.categories1
      } else if (!this.$store.state.InitialSetup.baseData.hasKeyword) {
        return Windows.keywords1
      } else {
        return Windows.finish
      }
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
    countries() {
      return sortBy(this.$store.state.Country.countries, c => c.countryName)
    },
    countryErrors() {
      const errors = []

      if (!this.$v.selectedCountry.$dirty) return errors

      !this.$v.selectedCountry.required && errors.push('A country is required')
      return errors
    },
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
      !this.$v.location.required && errors.push('A location is required')
      return errors
    },
    busy() {
      return this.$store.state.Location.busy
    }
  }
}
</script>

<style scoped>
</style>
