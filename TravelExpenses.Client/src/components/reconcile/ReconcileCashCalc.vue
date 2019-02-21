<template>
  <div>
    <h3 class="white--text">Cash calculator</h3>
    <v-select
      :items="currencies"
      v-model="currency"
      return-object
      :error-messages="currencyErrors"
      box
      background-color="white"
      color="primary"
      label="Currency"
      @input="$v.currency.$touch()"
      @blur="$v.currency.$touch()"
    >
      <template slot="selection" slot-scope="data">
        <div>
          <span>
            <strong>{{ data.item.isoCode }}</strong>
            - {{ data.item.currencyName }}
          </span>
        </div>
      </template>
      <template slot="item" slot-scope="data">
        <div>
          <span>
            <strong>{{ data.item.isoCode }}</strong>
            - {{ data.item.currencyName }}
          </span>
        </div>
      </template>
    </v-select>
    <v-select
      :items="locations"
      v-model="location"
      return-object
      :error-messages="locationErrors"
      box
      background-color="white"
      color="primary"
      label="Location"
      @input="$v.location.$touch()"
      @blur="$v.location.$touch()"
    >
      <template slot="selection" slot-scope="data">{{
        getLocationString(data.item)
      }}</template>
      <template slot="item" slot-scope="data">{{
        getLocationString(data.item)
      }}</template>
    </v-select>
    <enter-amount :currency="currency" />
    <v-btn @click="navToSummary">Done</v-btn>
  </div>
</template>

<script>
import Windows from '@/common/enums/ReconcileWindows.js'
import EnterAmount from '@/components/EnterAmount.vue'
import { required } from 'vuelidate/lib/validators'
import sortBy from 'lodash/sortBy'

export default {
  components: {
    EnterAmount
  },
  data() {
    return {
      currency: {},
      location: {}
    }
  },
  validations() {
    const result = {
      currency: {
        required
      },
      location: {
        required
      }
    }

    return result
  },
  methods: {
    navToSummary() {
      this.$store.dispatch('Reconcile/setReconcileWindowId', Windows.summary)
    },
    getLocationString(locationObj) {
      const country = this.$store.getters['Country/findCountry'](
        locationObj.countryId
      )

      if (country) {
        return `${locationObj.locationName}, ${country.countryName}`
      } else {
        return locationObj.locationName
      }
    }
  },
  computed: {
    currencyErrors() {
      const errors = []

      if (!this.$v.currency.$dirty) return errors

      !this.$v.currency.required && errors.push('A currency is required')
      return errors
    },
    locationErrors() {
      const errors = []

      if (!this.$v.location.$dirty) return errors

      !this.$v.location.required && errors.push('A location is required')
      return errors
    },
    currencies() {
      return this.$store.state.Currency.currencies
    },
    locations() {
      return sortBy(this.$store.state.Location.locations, l => l.locationName)
    }
  }
}
</script>

<style scoped></style>
