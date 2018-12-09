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
        v-model.trim="amount"
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
              <strong>{{data.item.isoCode}}</strong>
              - {{data.item.currencyName}}
            </span>
          </div>
        </template>
        <template slot="item" slot-scope="data">
          <div>
            <span>
              <strong>{{data.item.isoCode}}</strong>
              - {{data.item.currencyName}}
            </span>
          </div>
        </template>
      </v-select>
      <v-select
        :items="categories"
        v-model="category"
        return-object
        :error-messages="categoryErrors"
        box
        background-color="white"
        color="primary"
        label="Category"
        @input="$v.category.$touch()"
        @blur="$v.category.$touch()"
      >
        <template slot="selection" slot-scope="data">{{ data.item.categoryName }}</template>
        <template slot="item" slot-scope="data">{{ data.item.categoryName }}</template>
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
        <template slot="selection" slot-scope="data">{{ getLocationString(data.item) }}</template>
        <template slot="item" slot-scope="data">{{ getLocationString(data.item) }}</template>
      </v-select>
      <v-combobox :items="keywords" v-model="chosenKeywords" label="Keywords" chips solo multiple>
        <template slot="selection" slot-scope="data">
          <v-chip :selected="data.selected" close @input="removeKeyword(data.item)">
            <span>{{ data.item.keywordName }}</span>
          </v-chip>
        </template>
        <template slot="item" slot-scope="data">
          <v-checkbox
            v-model="chosenKeywords"
            color="primary"
            :value="data.item"
            :label="data.item.keywordName"
          />
        </template>
      </v-combobox>
      <v-textarea solo label="Description" auto-grow v-model="memo"></v-textarea>
      <v-checkbox dark v-model="paidWithCash">
        <div slot="label" class="white--text">Paid With Cash</div>
      </v-checkbox>
      <v-btn @click="cancel">Cancel</v-btn>
    </v-container>
  </div>
</template>

<script>
import {
  required,
  minLength,
  maxLength,
  decimal,
  minValue,
  maxValue
} from 'vuelidate/lib/validators'

import sortBy from 'lodash/sortBy'

export default {
  data() {
    return {
      title: '',
      date: '',
      dateMenu: false,
      amount: '',
      currency: {},
      category: {},
      location: {},
      chosenKeywords: [],
      memo: '',
      paidWithCash: true
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
        maxValue: maxValue(214748)
      },
      currency: {
        required
      },
      category: {
        required
      },
      location: {
        required
      }
    }

    return result
  },
  methods: {
    getLocationString(locationObj) {
      const country = this.countries.find(c => c.id === locationObj.countryId)

      return `${locationObj.locationName}, ${country.countryName}`
    },
    removeKeyword(item) {
      this.chosenKeywords.splice(this.chosenKeywords.indexOf(item), 1)
      this.chosenKeywords = [...this.chosenKeywords]
    },
    cancel() {
      this.$emit('done')
    }
  },
  computed: {
    currencies() {
      return sortBy(this.$store.state.SetupData.currencies, c => c.isoCode)
    },
    categories() {
      return sortBy(this.$store.state.SetupData.categories, c => c.categoryName)
    },
    locations() {
      return sortBy(this.$store.state.SetupData.locations, l => l.locationName)
    },
    countries() {
      return sortBy(this.$store.state.SetupData.countries, c => c.countryName)
    },
    keywords() {
      return sortBy(this.$store.state.SetupData.keywords, k => k.keywordName)
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
    categoryErrors() {
      const errors = []

      if (!this.$v.category.$dirty) return errors

      !this.$v.category.required && errors.push('A category is required')
      return errors
    },
    locationErrors() {
      const errors = []

      if (!this.$v.location.$dirty) return errors

      !this.$v.location.required && errors.push('A location is required')
      return errors
    }
  }
}
</script>

<style scoped>
</style>
