import React from "react";

const FilterForm = ({ filters, onFilterChange }) => (
    <form className="filters-form">
        <h3>Filters</h3>
        {/* <input
            type="text"
            name="searchText"
            placeholder="Search Text"
            value={filters.searchText}
            onChange={onFilterChange}
        /> */}
        <input
            type="text"
            name="Company"
            placeholder="Company"
            value={filters.companyName}
            onChange={onFilterChange}
        />
        {/* <input
            type="text"
            name="regon"
            placeholder="REGON"
            value={filters.regon}
            onChange={onFilterChange}
        /> */}
        <input
            type="text"
            name="Wojewodstwo"
            placeholder="Wojewodztwo"
            value={filters.wojewodstwo}
            onChange={onFilterChange}
        />
        <input
            type="text"
            name="Dzielnica"
            placeholder="Division"
            value={filters.divisionName}
            onChange={onFilterChange}
        />
        <input
            type="text"
            name="streetName"
            placeholder="Street"
            value={filters.streetName}
            onChange={onFilterChange}
        />
        publishFrom
        <input
            type="date"
            name="publishFrom"
            placeholder="Publish From"
            value={filters.publishFrom}
            onChange={onFilterChange}
        />
        publishTo
        <input
            type="date"
            name="publishTo"
            placeholder="Publish To"
            value={filters.publishTo}
            onChange={onFilterChange}
        />
        <input
            type="number"
            name="minSalary"
            placeholder="Min Salary"
            value={filters.minSalary}
            onChange={onFilterChange}
        />
        <input
            type="number"
            name="maxSalary"
            placeholder="Max Salary"
            value={filters.maxSalary}
            onChange={onFilterChange}
        />
        <label>
            For Students:
            <input
                type="checkbox"
                name="isForStudents"
                checked={filters.isForStudents}
                onChange={onFilterChange}
            />
        </label>
        <label>
            Negotiated Salary:
            <input
                type="checkbox"
                name="isNegotiatedSalary"
                checked={filters.isNegotiatedSalary}
                onChange={onFilterChange}
            />
        </label>
        <label>
            Sort:
        <select name="orderBy" value={filters.orderBy} onChange={onFilterChange}>
            <option value="publishStart">Publish Start</option>
            <option value="salaryMin">Min Salary</option>
            <option value="salaryMax">Max Salary</option>
        </select>
        <select name="ascending" value={filters.ascending} onChange={onFilterChange}>
            <option value={true}>Ascending</option>
            <option value={false}>Descending</option>
        </select>
        </label>
    </form>
);

export default FilterForm;
