
export class OptionsData {
    constructor(public timeStamp: string,
        public sourceName: string,
        public sourceValue: string,
        public expiryDate: string,
        public total_cal_OI: string,
        public total_cal_ChnginOI: string,
        public total_cal_Volume: string,
        public total_put_OI: string,
        public total_put_ChnginOI: string,
        public total_put_Volume: string,
        public sourceType: string,
        public percentage: string,
        public underlyingValue: string,
        public asOnTime: string
    ) {

    }
}