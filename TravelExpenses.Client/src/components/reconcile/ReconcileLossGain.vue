<template>
  <v-container>
    <v-card class="pb-2">
      <v-card-title class="pb-0">
        <v-layout row>
          <v-flex>
            <v-avatar class="mr-3" size="55" color="primary">
              <v-icon dark large>
                {{ haveNetGain ? 'trending_up' : 'trending_down' }}
              </v-icon>
            </v-avatar>
          </v-flex>
          <v-flex>
            <h2>Loss / Gain Adjustment</h2>
            <p>
              {{
                `${resultString} and are recording a ${
                  haveNetGain ? 'gain' : 'loss'
                }.`
              }}
            </p>
          </v-flex>
        </v-layout>
      </v-card-title>
      <h3 class="ml-3">Description</h3>
      <v-textarea
        class="ma-2 elevation-7"
        hide-details
        solo
        label="Enter a description."
        :readonly="saved"
        auto-grow
        v-model="memo"
        @input="$v.memo.$touch()"
        @blur="$v.memo.$touch()"
      ></v-textarea>
    </v-card>
    <v-layout class="mt-3" column justify-center align-center>
      <v-btn
        class="mb-3"
        dark
        color="primary"
        :disabled="$v.$invalid || saved"
        :loading="saveTransactionBusy"
        @click="saveAdjustment"
        >Save</v-btn
      >
      <v-btn
        dark
        color="primary"
        :disabled="saveTransactionBusy || saved"
        @click="returnToSummary"
        >Return to Summary</v-btn
      >
    </v-layout>
  </v-container>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import { required, minLength } from 'vuelidate/lib/validators'

export default {
  data() {
    return {
      memo: '',
      saved: false
    }
  },
  methods: {
    saveAdjustment() {
      let transactionToSave = {
        title: `Recorded ${this.haveNetGain ? 'gain' : 'loss'} adjustment`,
        transDate: this.reconcileSummary.lastTransactionDay,
        amount: this.haveNetGain ? this.amountOut * -1 : this.amountOut,
        locationId: this.location.id,
        currencyId: this.currency.id,
        categoryId: this.lossGainCategory.id,
        memo: this.memo,
        paidWithCash: true,
        userId: this.userId
      }

      this.$store
        .dispatch('Transaction/saveTransaction', {
          transaction: transactionToSave,
          complete: () => {}
        })
        .then(() => {
          this.saved = true
          this.$store.dispatch('showSaveMessage', 'Adjustment has been saved')
        })
    },
    returnToSummary() {
      this.$emit('returnToSummary')
    }
  },
  validations() {
    const result = {
      memo: {
        minLength: minLength(3),
        required
      }
    }

    return result
  },
  computed: {
    ...mapGetters('Reconcile', ['haveNetGain', 'resultString', 'amountOut']),
    ...mapState('Reconcile', ['reconcileSummary', 'location', 'currency']),
    ...mapState('Category', ['lossGainCategory']),
    ...mapGetters('Authentication', ['userId']),
    ...mapState('Transaction', ['saveTransactionBusy'])
  }
}
</script>

<style scoped></style>
