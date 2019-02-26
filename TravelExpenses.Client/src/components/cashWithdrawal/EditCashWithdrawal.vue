<template>
  <div id="root">
    <v-container>
      <v-text-field
        v-model.trim="title"
        :error-messages="titleErrors"
        label="Title"
        box
        background-color="white"
        color="primary"
        @input="$v.title.$touch()"
        @blur="$v.title.$touch()"
      ></v-text-field>
      <v-menu
        :close-on-content-click="false"
        v-model="dateMenu"
        :nudge-right="40"
        lazy
        transition="scale-transition"
        offset-y
        full-width
        min-width="290px"
      >
        <v-text-field
          slot="activator"
          v-model="date"
          :error-messages="dateErrors"
          box
          background-color="white"
          color="primary"
          label="Date"
          readonly
          @input="$v.date.$touch()"
          @blur="$v.date.$touch()"
        ></v-text-field>
        <v-date-picker v-model="date" @input="dateMenu = false"></v-date-picker>
      </v-menu>
      <v-text-field
        v-model.number="amount"
        :error-messages="amountErrors"
        label="Amount"
        box
        background-color="white"
        color="primary"
        @input="$v.amount.$touch()"
        @blur="$v.amount.$touch()"
      ></v-text-field>
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
        <template slot="selection" slot-scope="data">
          {{ getLocationString(data.item) }}
        </template>
        <template slot="item" slot-scope="data">
          {{ getLocationString(data.item) }}
        </template>
      </v-select>
      <v-textarea
        hide-details
        solo
        label="Description"
        auto-grow
        v-model="memo"
        @input="$v.memo.$touch()"
        @blur="$v.memo.$touch()"
      ></v-textarea>
      <v-flex xs10 offset-xs1>
        <v-layout justify-center justify-space-between>
          <v-btn
            dark
            :loading="busy"
            :disabled="$v.$invalid || busy || !$v.$anyDirty"
            @click="save"
            >Save</v-btn
          >
          <v-btn dark @click="cancel">Cancel</v-btn>
        </v-layout>
      </v-flex>
    </v-container>
  </div>
</template>

<script>
/* eslint-disable no-console */
import {
  required,
  minLength,
  maxLength,
  decimal,
  minValue,
  maxValue
} from 'vuelidate/lib/validators'

import sortBy from 'lodash/sortBy'
import { mapGetters } from 'vuex'

export default {
  props: {
    edit: Boolean
  },
  created() {
    if (!this.$store.state.Currency.currencies.length) {
      this.$store.dispatch('Currency/load')
    }
  },
  data() {
    return {
      id: 0,
      title: '',
      date: '',
      dateMenu: false,
      amount: '',
      currency: {},
      location: {},
      memo: ''
    }
  },
  validations() {
    const result = {
      title: {
        required,
        minLength: minLength(3),
        maxLength: maxLength(255)
      },
      date: {
        required
      },
      amount: {
        required,
        decimal,
        minValue: minValue(0.01),
        maxValue: maxValue(922337203685477.58)
      },
      currency: {
        required
      },
      location: {
        required
      },
      memo: {}
    }

    return result
  },
  methods: {
    getLocationString(locationObj) {
      const country = this.$store.getters['Country/findCountry'](
        locationObj.countryId
      )

      if (country) {
        return `${locationObj.locationName}, ${country.countryName}`
      } else {
        return locationObj.locationName
      }
    },
    leave() {
      if (this.$store.state.CashWithdrawal.recentCashWithdrawalsStale) {
        this.$store.dispatch('CashWithdrawal/reloadRecentCashWithdrawals')
      }
      this.$emit('done')
    },
    save() {
      let cashWithdrawalToSave = {
        title: this.title,
        transDate: this.date,
        amount: this.amount,
        currencyId: this.currency.id,
        locationId: this.location.id,
        memo: this.memo,
        userId: this.userId
      }

      if (this.edit) {
        cashWithdrawalToSave['id'] = this.id
      }

      this.$store.dispatch(
        `CashWithdrawal/${this.edit ? 'edit' : 'save'}CashWithdrawal`,
        {
          cashWithdrawal: cashWithdrawalToSave,
          complete: () => {
            this.leave()
            this.resetForm()
          }
        }
      )
    },
    resetForm() {
      this.$v.$reset()

      this.title = ''
      //this.date = ''
      this.amount = ''
      //this.currency = {}
      this.memo = ''
    },
    cancel() {
      this.leave()
    }
  },
  computed: {
    ...mapGetters('Authentication', ['userId']),
    currencies() {
      return sortBy(this.$store.state.Currency.currencies, c => c.isoCode)
    },
    locations() {
      return sortBy(this.$store.state.Location.locations, l => l.locationName)
    },
    titleErrors() {
      const errors = []

      if (!this.$v.title.$dirty) return errors

      !this.$v.title.maxLength &&
        errors.push(
          `The title can be a maximum of ${
            this.$v.title.$params.maxLength.max
          } characters`
        )
      !this.$v.title.minLength &&
        errors.push(
          `The title must be a minimum of ${
            this.$v.title.$params.minLength.min
          } characters`
        )
      !this.$v.title.required && errors.push('A title is required')
      return errors
    },
    dateErrors() {
      const errors = []

      if (!this.$v.date.$dirty) return errors

      !this.$v.date.required && errors.push('A date is required')
      return errors
    },
    amountErrors() {
      const errors = []

      if (!this.$v.amount.$dirty) return errors

      !this.$v.amount.decimal && errors.push('The amount must numeric')

      !this.$v.amount.maxValue &&
        errors.push(
          `The amount can be a maximum of ${
            this.$v.amount.$params.maxValue.max
          }`
        )
      !this.$v.amount.minValue &&
        errors.push(
          `The amount must be a minimum of ${
            this.$v.amount.$params.minValue.min
          }`
        )

      !this.$v.amount.required && errors.push('An amount is required')
      return errors
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
    busy() {
      return this.$store.state.CashWithdrawal.saveCashWithdrawalBusy
    },
    cashWithdrawalToEdit() {
      return this.$store.state.CashWithdrawal.selectedCashWithdrawal
    }
  },
  watch: {
    cashWithdrawalToEdit(val) {
      if (this.edit && val.id) {
        this.id = val.id
        this.title = val.title
        this.date = val.transDate
        this.amount = val.amount
        this.currency = this.currencies.find(c => c.id === val.currencyId)
        this.memo = val.memo
        this.$v.$reset()
      }
    }
  }
}
</script>

<style scoped></style>
