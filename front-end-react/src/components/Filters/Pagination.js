import React from "react";

const Pagination = ({ currentPage, onPageChange, hasNext }) => (
    <div className="pagination">
        <button onClick={() => onPageChange(currentPage - 1)} disabled={currentPage === 1}>
            Previous
        </button>
        <span>Page: {currentPage}</span>
        <button onClick={() => onPageChange(currentPage + 1)} disabled={!hasNext}>
            Next
        </button>
    </div>
);

export default Pagination;
