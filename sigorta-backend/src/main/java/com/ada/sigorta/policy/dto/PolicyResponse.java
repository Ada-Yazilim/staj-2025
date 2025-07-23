package com.ada.sigorta.policy.dto;

import com.ada.sigorta.policy.model.PolicyType;
import lombok.Builder;
import lombok.Data;

import java.time.LocalDate;

@Data
@Builder
public class PolicyResponse {
    private Long id;
    private PolicyType type;
    private LocalDate startDate;
    private LocalDate endDate;
    private double premium;
    private boolean isPaid;
}

