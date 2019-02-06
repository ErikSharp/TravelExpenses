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
        <template slot="selection" slot-scope="data">{{
          data.item.categoryName
        }}</template>
        <template slot="item" slot-scope="data">{{
          data.item.categoryName
        }}</template>
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
      <v-select
        :items="keywords"
        v-model="chosenKeywords"
        label="Keywords"
        chips
        solo
        multiple
        @input="$v.chosenKeywords.$touch()"
        @blur="$v.chosenKeywords.$touch()"
      >
        <template slot="selection" slot-scope="data">
          <v-chip
            :selected="data.selected"
            close
            @input="removeKeyword(data.item)"
          >
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
          <v-switch
            hide-details
            class="my-2 justify-center"
            dark
            color="white"
            v-model="gpsLocation"
            validate-on-blur
            @change="$v.gpsLocation.$touch()"
          >
            <div
              slot="label"
              :class="{
                'white--text': gpsLocation,
                'gray--text': !gpsLocation
              }"
            >
              GPS Location
            </div>
          </v-switch>
          <v-checkbox
            hide-details
            class="my-2 justify-center"
            dark
            v-model="paidWithCash"
            @change="$v.paidWithCash.$touch()"
          >
            <div slot="label" class="white--text">Paid With Cash</div>
          </v-checkbox>
        </v-layout>
      </v-flex>
      <v-flex xs10 offset-xs1>
        <v-layout justify-center justify-space-between>
          <v-btn
            dark
            :loading="busy && !usingSaveAndNew"
            :disabled="
              $v.$invalid || (busy && !usingSaveAndNew) || !$v.$anyDirty
            "
            @click="save"
            >Save</v-btn
          >
          <v-btn
            v-if="!edit"
            dark
            :loading="busy && usingSaveAndNew"
            :disabled="$v.$invalid || (busy && usingSaveAndNew)"
            @click="saveAndNew"
            >Save & New</v-btn
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
    if (!this.$store.state.Country.countries.length) {
      this.$store.dispatch('Country/load')
    }
    if (!this.$store.state.Location.locations.length) {
      this.$store.dispatch('Location/load')
    }
    if (!this.$store.state.Currency.currencies.length) {
      this.$store.dispatch('Currency/load')
    }
    if (!this.$store.state.Keyword.keywords.length) {
      this.$store.dispatch('Keyword/load')
    }
    if (!this.$store.state.Category.categories.length) {
      this.$store.dispatch('Category/load')
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
      category: {},
      location: {},
      chosenKeywords: [],
      memo: '',
      gpsLocation: false,
      paidWithCash: true,
      usingSaveAndNew: false
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
      category: {
        required
      },
      location: {
        required
      },
      chosenKeywords: {},
      memo: {},
      gpsLocation: {},
      paidWithCash: {}
    }

    return result
  },
  methods: {
    getLocationString(locationObj) {
      const country = this.countries.find(c => c.id === locationObj.countryId)
      if (country) {
        return `${locationObj.locationName}, ${country.countryName}`
      } else {
        return locationObj.locationName
      }
    },
    removeKeyword(item) {
      this.chosenKeywords.splice(this.chosenKeywords.indexOf(item), 1)
      this.chosenKeywords = [...this.chosenKeywords]
    },
    saveInternal() {
      this.$store
        .dispatch('Transaction/saveTransaction', {
          title: this.title,
          transDate: this.date,
          amount: this.amount,
          locationId: this.location.id,
          currencyId: this.currency.id,
          categoryId: this.category.id,
          memo: this.memo,
          paidWithCash: this.paidWithCash,
          userId: this.userId,
          keywordIds: this.chosenKeywords.map(k => k.id)
        })
        .then(() => {
          if (!this.usingSaveAndNew) {
            this.leave()
          }
          this.resetForm()
        })
    },
    leave() {
      if (this.$store.state.Transaction.recentTransactionsStale) {
        this.$store.dispatch('Transaction/reloadRecentTransactions')
      }
      this.$emit('done')
    },
    save() {
      this.usingSaveAndNew = false
      this.saveInternal()
    },
    saveAndNew() {
      this.usingSaveAndNew = true
      this.saveInternal()
    },
    resetForm() {
      this.$v.$reset()

      this.title = ''
      //this.date = ''
      this.amount = ''
      //this.currency = {}
      this.category = {}
      //this.location = {}
      this.chosenKeywords = []
      this.memo = ''
    },
    cancel() {
      this.leave()
    },
    scrollToTop() {
      this.$vuetify.goTo(0, {
        duration: 300,
        offset: 0,
        easing: 'easeInOutCubic'
      })
    }
  },
  computed: {
    ...mapGetters('Authentication', ['userId']),
    currencies() {
      return sortBy(this.$store.state.Currency.currencies, c => c.isoCode)
    },
    categories() {
      return sortBy(this.$store.state.Category.categories, c => c.categoryName)
    },
    locations() {
      return sortBy(this.$store.state.Location.locations, l => l.locationName)
    },
    countries() {
      return sortBy(this.$store.state.Country.countries, c => c.countryName)
    },
    keywords() {
      return sortBy(this.$store.state.Keyword.keywords, k => k.keywordName)
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
    },
    busy() {
      const isBusy = this.$store.state.Transaction.saveTransactionBusy

      if (!isBusy && this.usingSaveAndNew) {
        this.scrollToTop()
      }

      return isBusy
    },
    transactionToEdit() {
      return this.$store.state.Transaction.selectedTransaction
    }
  },
  watch: {
    transactionToEdit(val) {
      if (this.edit && val.id) {
        this.id = val.id
        this.title = val.title
        this.date = val.transDate
        this.amount = val.amount
        this.currency = this.currencies.find(c => c.id === val.currencyId)
        this.category = this.categories.find(c => c.id === val.categoryId)
        this.location = this.locations.find(l => l.id === val.locationId)
        this.chosenKeywords = val.keywordIds.map(id =>
          this.keywords.find(k => k.id === id)
        )
        this.memo = val.memo
        this.gpsLocation = val.gpsLocation
        this.paidWithCash = val.paidWithCash
        this.$v.$reset()
      }
    }
  }
}
</script>

<style scoped></style>
