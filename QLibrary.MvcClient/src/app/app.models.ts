export class Section {
    SectionId: number;
    SectionName: string;
    CreatedDate: Date;
}
export class Query {
    QueryId: number;
    SectionId: number;
    QueryName: string;
    Description: string;
    SCreatedDate: Date;
    //SCreatedDate: Department;
    constructor() {
    }
}


 