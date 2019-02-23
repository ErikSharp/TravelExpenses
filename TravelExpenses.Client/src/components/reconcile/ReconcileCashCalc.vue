<template>
  <div>
    <h3 class="white--text mb-2">Cash calculator</h3>
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
    <v-layout justify-end align-center>
      <h3 class="white--text mb-0 mr-3">Cash on hand</h3>
      <enter-amount
        :currency="currency"
        :buttonText="getAmountButtonText"
        @amountEntered="onAmountEntered($event)"
      />
    </v-layout>
    <v-layout justify-center>
      <v-btn
        @click="beginReconcile"
        :loading="reconcileBusy"
        :disabled="$v.$invalid"
        dark
        class="primary mt-5"
        >RECONCILE</v-btn
      >
    </v-layout>
  </div>
</template>

<script>
import Windows from '@/common/enums/ReconcileWindows.js'
import { toLocaleStringWithEndingZero } from '@/common/StringUtilities.js'
import EnterAmount from '@/components/EnterAmount.vue'
import { required, minValue } from 'vuelidate/lib/validators'
import sortBy from 'lodash/sortBy'
import { mapState } from 'vuex'

export default {
  components: {
    EnterAmount
  },
  validations() {
    const result = {
      currency: {
        required
      },
      location: {
        required
      },
      cashOnHand: {
        minValue: minValue(0.001)
      }
    }

    return result
  },
  methods: {
    onAmountEntered(amount) {
      this.$store.dispatch('Reconcile/setCashOnHand', amount)
    },
    beginReconcile() {
      this.$store.dispatch('Reconcile/getReconcileSummary', () => {
        this.$store.dispatch('Reconcile/setReconcileWindowId', Windows.summary)
      })
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
    ...mapState('Currency', ['currencies']),
    ...mapState('Reconcile', ['cashOnHand', 'reconcileBusy']),
    getAmountButtonText() {
      return this.$store.state.Reconcile.cashOnHand
        ? toLocaleStringWithEndingZero(this.$store.state.Reconcile.cashOnHand)
        : 'ENTER AMOUNT'
    },
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
    cashOnHandErrors() {
      const errors = []

      if (!this.$v.cashOnHand.$dirty) return errors

      !this.$v.cashOnHand.numeric && errors.push('The amount must be numeric')

      !this.$v.cashOnHand.minValue &&
        errors.push('The amount must be greater than zero')

      return errors
    },
    currency: {
      get() {
        return this.$store.state.Reconcile.currency
      },
      set(value) {
        this.$store.dispatch('Reconcile/setCurrency', value)
      }
    },
    location: {
      get() {
        return this.$store.state.Reconcile.location
      },
      set(value) {
        this.$store.dispatch('Reconcile/setLocation', value)
      }
    },
    locations() {
      return sortBy(this.$store.state.Location.locations, l => l.locationName)
    }
  }
}
</script>

<style scoped></style>
