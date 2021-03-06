<template>
  <v-container>
    <h2 class="white--text mb-2">Cash calculator</h2>
    <v-autocomplete
      :items="currencies"
      v-model="currency"
      :filter="filterCurrency"
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
    </v-autocomplete>
    <v-layout justify-end align-center>
      <h3 class="white--text mb-0 mr-3">Cash on hand</h3>
      <enter-amount
        :buttonText="getAmountButtonText"
        @amountEntered="onAmountEntered($event)"
      />
    </v-layout>
    <v-layout justify-center>
      <v-btn
        @click="beginReconcile"
        :loading="reconcileBusy"
        :disabled="$v.$invalid"
        flat
        class="primary mt-4"
        >RECONCILE</v-btn
      >
    </v-layout>
  </v-container>
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
      cashOnHand: {
        minValue: minValue(0.001)
      }
    }

    return result
  },
  methods: {
    filterCurrency(item, queryText) {
      if (queryText.trim() === '') {
        return true
      } else {
        return item.isoCode.toLowerCase().indexOf(queryText.toLowerCase()) > -1
      }
    },
    onAmountEntered(amount) {
      this.$store.dispatch('Reconcile/setCashOnHand', amount)
    },
    beginReconcile() {
      this.$store.dispatch('Reconcile/getReconcileSummary').then(() => {
        this.$store.dispatch('Reconcile/setReconcileWindowId', Windows.summary)
      })
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
    }
  }
}
</script>

<style scoped></style>
