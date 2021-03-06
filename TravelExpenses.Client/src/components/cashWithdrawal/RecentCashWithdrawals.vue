<template>
  <div>
    <div
      class="helper-text"
      v-if="!Object.keys(cashWithdrawals).length && !cashWithdrawalsBusy"
    >
      <h1 class="text-xs-center white--text">No Cash Withdrawals</h1>
      <p class="text-xs-center white--text">
        Press the Add button below to create your first.
      </p>
    </div>
    <v-layout
      v-show="!Object.keys(cashWithdrawals).length && cashWithdrawalsBusy"
      justify-center
    >
      <v-progress-circular
        class="mt-5"
        size="70"
        width="7"
        color="accent"
        indeterminate
      ></v-progress-circular>
    </v-layout>
    <v-expansion-panel dark v-model="panels" expand>
      <v-expansion-panel-content
        class="primary withdrawals"
        v-for="(dateGroup, date) in cashWithdrawals"
        :key="date"
      >
        <div slot="header">{{ getDateString(date) }}</div>
        <div style="background: var(--v-secondary-base)" class="py-1 px-2">
          <cash-withdrawal-card
            v-for="(cashWithdrawal, i) in dateGroup"
            :key="cashWithdrawal.id"
            :cashWithdrawal="cashWithdrawal"
          />
        </div>
      </v-expansion-panel-content>
    </v-expansion-panel>
    <div class="bottom-spacer"></div>
    <v-flex class="button-background" xs12 v-show="!cashWithdrawalsBusy">
      <div class="text-xs-center my-1">
        <v-pagination
          v-model="page"
          total-visible="6"
          :length="pageCount"
        ></v-pagination>
      </div>
      <v-flex xs12 sm10 offset-sm1>
        <v-layout justify-center justify-space-between>
          <v-btn flat class="primary mb-2 mt-0" @click="addCashWithdrawal">
            <v-icon dark>add</v-icon>Add
          </v-btn>
          <v-btn
            flat
            :disabled="!cashWithdrawalSelected"
            class="primary mb-2 mt-0"
            @click="editCashWithdrawal"
          >
            <v-icon dark>edit</v-icon>Edit
          </v-btn>
          <v-dialog
            v-model="deleteDialog"
            :disabled="!cashWithdrawalSelected"
            persistent
            max-width="290"
          >
            <v-btn
              slot="activator"
              flat
              :disabled="!cashWithdrawalSelected"
              class="primary mb-2 mt-0"
            >
              <v-icon dark>delete</v-icon>Delete
            </v-btn>
            <v-card>
              <v-avatar size="70" class="mx-2 elevation-5 red">
                <v-icon size="45" class="white--text">warning</v-icon>
              </v-avatar>
              <v-card-title class="headline"
                >Delete Cash Withdrawal</v-card-title
              >
              <v-card-text
                >There is no way to undo this procedure. Do you wish to
                proceed?</v-card-text
              >
              <v-card-actions>
                <v-layout justify-space-around>
                  <v-btn
                    color="red"
                    :loading="saveCashWithdrawalBusy"
                    dark
                    @click="deleteCashWithdrawal"
                    >YES</v-btn
                  >
                  <v-btn
                    color="primary"
                    :disabled="saveCashWithdrawalBusy"
                    @click="deleteDialog = false"
                    >NO</v-btn
                  >
                </v-layout>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-layout>
      </v-flex>
    </v-flex>
  </div>
</template>

<script>
/* eslint-disable no-console */
import groupBy from 'lodash/groupBy'
import CashWithdrawalCard from '@/components/cashWithdrawal/CashWithdrawalCard.vue'
import { mapState, mapGetters } from 'vuex'

const dateOptions = {
  weekday: 'long',
  year: 'numeric',
  month: 'short',
  day: '2-digit'
}

const locale = navigator.language || 'en-US'

export default {
  components: {
    CashWithdrawalCard
  },
  data() {
    return {
      panels: [true],
      deleteDialog: false
    }
  },
  methods: {
    addCashWithdrawal() {
      this.$emit('addCashWithdrawal')
    },
    editCashWithdrawal() {
      this.$emit('editCashWithdrawal')
    },
    deleteCashWithdrawal() {
      this.$store.dispatch(
        'CashWithdrawal/deleteSelectedCashWithdrawal',
        () => {
          this.deleteDialog = false
        }
      )
    },
    getDateString(date) {
      return new Date(date).toLocaleDateString(locale, dateOptions)
    }
  },
  computed: {
    ...mapState('CashWithdrawal', [
      'cashWithdrawalsBusy',
      'saveCashWithdrawalBusy',
      'selectedCashWithdrawal',
      'totalRecords',
      'pageSize'
    ]),
    ...mapGetters('CashWithdrawal', ['pageCount']),
    page: {
      get() {
        return this.$store.state.CashWithdrawal.page
      },
      set(val) {
        this.$store.dispatch('CashWithdrawal/getCashWithdrawals', val)
      }
    },
    cashWithdrawals() {
      return groupBy(
        this.$store.state.CashWithdrawal.cashWithdrawals,
        t => t.transDate
      )
    },
    cashWithdrawalSelected() {
      return !!this.selectedCashWithdrawal.title
    }
  },
  watch: {
    cashWithdrawals(val) {
      this.panels = new Array(Object.keys(val).length)
      this.panels.fill(false)

      if (this.panels.length) {
        this.panels[0] = true
      }
    }
  }
}
</script>

<style scoped>
.button-background {
  position: fixed;
  width: 100%;
  bottom: 56px;
  background: rgba(0, 0, 0, 0.8);
}

.withdrawals:first-child {
  margin-top: 0;
}

.withdrawals {
  margin-top: 2px;
}

.bottom-spacer {
  height: 95px;
}

.helper-text {
  margin-top: 75px;
}

>>> .v-pagination__more {
  color: white !important;
}
</style>
